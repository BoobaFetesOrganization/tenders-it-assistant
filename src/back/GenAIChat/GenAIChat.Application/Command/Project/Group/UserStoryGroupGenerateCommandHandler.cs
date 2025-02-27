using GenAIChat.Application.Adapter.Api;
using GenAIChat.Application.Command.Common;
using GenAIChat.Application.Resources;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Gemini;
using GenAIChat.Domain.Gemini.GeminiCommon;
using GenAIChat.Domain.Project;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Domain.Project.Group.UserStory;
using MediatR;
using System.Text;
using System.Text.Json;

namespace GenAIChat.Application.Command.Project.Group
{
    public class UserStoryGroupGenerateCommand : IRequest<UserStoryGroupDomain>
    {
        public required string ProjectId { get; init; }
        public required string GroupId { get; init; }
    }

    public class UserStoryGroupGenerateCommandHandler(IMediator mediator, IGenAiApiAdapter genAiAdapter, EmbeddedResource resources) : IRequestHandler<UserStoryGroupGenerateCommand, UserStoryGroupDomain>
    {
        public async Task<UserStoryGroupDomain> Handle(UserStoryGroupGenerateCommand request, CancellationToken cancellationToken)
        {
            // check constraints
            var project = await mediator.Send(new GetByIdQuery<ProjectDomain> { Id = request.ProjectId }, cancellationToken);
            if (project is null) throw new Exception($"{nameof(ProjectDomain)} '{request.ProjectId}' is not found ");

            var domain = await mediator.Send(new GetByIdQuery<UserStoryGroupDomain> { Id = request.GroupId }, cancellationToken);
            if (domain is null) throw new Exception($"{nameof(UserStoryGroupDomain)} '{request.GroupId}' is not found ");

            // actions
            IEnumerable<DocumentDomain> documents = await RehydrateDocuments(domain, cancellationToken);
            domain.Response = await SendRequestToGenAi(domain, documents);
            GeminiResponse response = GeminiResponse.LoadFrom(domain.Response);

            // clean up
            await DeleteOldStories(domain, cancellationToken);
            /* c'est à faire dans la couche infra */ await CreateGeneratedStories(domain, response, cancellationToken);
            await ResetSelectedGroupIfNeeded(project, domain, cancellationToken);

            await mediator.Send(new UpdateCommand<UserStoryRequestDomain> { Domain = domain.Request }, cancellationToken);
            return await mediator.Send(new UpdateCommand<UserStoryGroupDomain> { Domain = domain }, cancellationToken);
        }

        #region remote actions

        private async Task<IEnumerable<DocumentDomain>> RehydrateDocuments(UserStoryGroupDomain domain, CancellationToken cancellationToken)
        {
            // upload files to the GenAI and store new Metadata
            var filter = new PropertyEqualsFilter(nameof(UserStoryGroupDomain.ProjectId), domain.ProjectId);
            IEnumerable<DocumentDomain> documents = await mediator.Send(new GetAllQuery<DocumentDomain>() { Filter = filter }, cancellationToken);
            if (!documents.Any()) throw new Exception("To generate user stories, at least one document is required");

            var uploads = await genAiAdapter.SendFilesAsync(documents);
            await Task.WhenAll(uploads.Select(doc => mediator.Send(new UpdateCommand<DocumentDomain>() { Domain = doc }, cancellationToken)));

            return documents;
        }

        private async Task<string> SendRequestToGenAi(UserStoryGroupDomain domain, IEnumerable<DocumentDomain> documents)
        {
            GeminiContent content = new();
            content.AddPart(ConvertRequestDomainToGeminiContent(domain.Request));
            if (documents is not null) content.AddParts(documents.Select(document => new GeminiContentPart(document.Metadata.MimeType, document.Metadata.Uri)));

            GeminiRequest data = new();
            data.AddContent(content);

            data.SetGenerationConfig(CreateResultFromSchema(resources.UserStoryRequestSchema));

            return await genAiAdapter.SendRequestAsync(data);
        }
        #region SendRequestToGenAi(...) helpers

        private static GeminiContentPart ConvertRequestDomainToGeminiContent(UserStoryRequestDomain domain)
        {
            StringBuilder sb = new();
            void append(string key, string value)
            {
                sb.Append($"{key}:");
                sb.Append($"{value}{Environment.NewLine}");
            }

            append(nameof(domain.Context), domain.Context);
            append(nameof(domain.Personas), domain.Personas);
            append(nameof(domain.Tasks), domain.Tasks);

            return new(sb.ToString());
        }
        public static GeminiGenerationConfig CreateResultFromSchema(object? schema) => new()
        {
            MimeType = "application/json",
            Schema = schema ?? throw new ArgumentNullException(nameof(schema), "the parameter should not be null")
        };

        #endregion

        #endregion

        #region clean up

        private async Task DeleteOldStories(UserStoryGroupDomain domain, CancellationToken cancellationToken)
        {
            var oldStoriesFilter = new PropertyEqualsFilter(nameof(UserStoryDomain.GroupId), domain.Id);
            var oldStories = await mediator.Send(new GetAllQuery<UserStoryDomain> { Filter = oldStoriesFilter }, cancellationToken);
            await Task.WhenAll(oldStories.Select(
                 story => mediator.Send(new DeleteCommand<UserStoryDomain>() { Domain = story }, cancellationToken)));
            domain.ClearUserStories();
        }

        private async Task CreateGeneratedStories(UserStoryGroupDomain domain, GeminiResponse response, CancellationToken cancellationToken)
        {
            
            // warning if EntityFramework is used, this will delete the stories from the database and cascading to all dependent entities

            domain.SetUserStory(ExtractUserStories(response));
            await Task.WhenAll(domain.UserStories.Select(
                story => mediator.Send(new CreateCommand<UserStoryDomain>() { Domain = story }, cancellationToken)));
        }
        #region CreateGeneratedStories(...) helpers

        private static ICollection<UserStoryDomain> ExtractUserStories(GeminiResponse response)
        {
            var responseResult = response.Candidates.FirstOrDefault(i => i.Content.Role == "model")?.Content?.Parts.Last()
                ?? throw new InvalidOperationException("Error while getting the response text from the payload");

            var text = responseResult.Text
                ?? throw new InvalidOperationException("Error while getting the text result from the payload");

            // load user stories from the GenAI result
            var userStories = JsonSerializer.Deserialize<ICollection<UserStoryDomain>>(text) ?? [];

            // set gemini cost and remove selected cost
            var tasks = userStories.SelectMany(us => us.Tasks);
            foreach (var task in tasks)
            {
                task.AddGeminiCost(task.Cost);
                task.Cost = 0;
            }

            return userStories;
        }

        #endregion

        private async Task ResetSelectedGroupIfNeeded(ProjectDomain project, UserStoryGroupDomain domain, CancellationToken cancellationToken)
        {
            // when the selected group of the project is generated (or regenerated),
            // the selection must be unset

            if (project.SelectedGroupId is null || project.SelectedGroupId != domain.Id) return;

            project.SelectedGroupId = null;
            await mediator.Send(new UpdateCommand<ProjectDomain> { Domain = project }, cancellationToken);
        }

        #endregion
    }
}
