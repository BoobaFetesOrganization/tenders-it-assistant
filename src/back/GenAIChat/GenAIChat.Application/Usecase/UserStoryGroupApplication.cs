using GenAIChat.Application.Adapter.Api;
using GenAIChat.Application.Command.Common;
using GenAIChat.Application.Resources;
using GenAIChat.Application.Usecase.Interface;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Gemini;
using GenAIChat.Domain.Gemini.GeminiCommon;
using GenAIChat.Domain.Project;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Domain.Project.Group.UserStory;
using GenAIChat.Domain.Project.Group.UserStory.Task.Cost;
using MediatR;
using System.Text;
using System.Text.Json;

namespace GenAIChat.Application.Usecase
{
#pragma warning disable CS9107 // Un paramètre est capturé dans l’état du type englobant et sa valeur est également passée au constructeur de base. La valeur peut également être capturée par la classe de base.
    public class UserStoryGroupApplication(EmbeddedResource resources, IGenAiApiAdapter genAiAdapter, IMediator mediator) : ApplicationBase<UserStoryGroupDomain>(mediator), IUserStoryGroupApplication
#pragma warning restore CS9107 // Un paramètre est capturé dans l’état du type englobant et sa valeur est également passée au constructeur de base. La valeur peut également être capturée par la classe de base.
    {
        public override Task<UserStoryGroupDomain> CreateAsync(UserStoryGroupDomain domain, CancellationToken cancellationToken = default) => CreateAsync(domain.ProjectId, cancellationToken);

        public async Task<UserStoryGroupDomain> CreateAsync(string projectId, CancellationToken cancellationToken = default)
        {
            var project = await mediator.Send(new GetByIdQuery<ProjectDomain>() { Id = projectId }, cancellationToken) ?? throw new Exception("Project not found");

            UserStoryGroupDomain group = await base.CreateAsync(new()
            {
                ProjectId = projectId,
                Request = (UserStoryRequestDomain)resources.UserStoryRequest.Clone()
            }, cancellationToken);

            try
            {
                var item = await GenerateUserStoriesAsync(project, group, cancellationToken);
            }
            catch (Exception ex)
            {
                // silent catch, because it's not required to get the generation of user stories done right now..
                Console.WriteLine(ex.Message);
            }

            return (await GetByIdAsync(group.Id, cancellationToken)) ?? throw new Exception("new item not found !");
        }

        public async Task<UserStoryGroupDomain> GenerateUserStoriesAsync(string projectId, string groupId, CancellationToken cancellationToken = default) => await GenerateUserStoriesAsync(
               await mediator.Send(new GetByIdQuery<ProjectDomain>() { Id = projectId }, cancellationToken),
               await mediator.Send(new GetByIdQuery<UserStoryGroupDomain>() { Id = groupId }, cancellationToken),
               cancellationToken);

        private async Task<UserStoryGroupDomain> GenerateUserStoriesAsync(ProjectDomain? _project, UserStoryGroupDomain? _group, CancellationToken cancellationToken = default)
        {
            var project = _project ?? throw new Exception("Project  not found");
            var group = _group ?? throw new Exception("Group not found");

            // actions
            IEnumerable<DocumentDomain> documents = await RehydrateDocuments(group, cancellationToken);
            group.Response = await SendRequestToGenAi(group, documents, cancellationToken);
            GeminiResponse response = GeminiResponse.LoadFrom(group.Response);

            // set values of the group
            List<Task> actions = [mediator.Send(new UpdateCommand<UserStoryRequestDomain> { Domain = group.Request }, cancellationToken)];
            group.ClearUserStories();
            CreateGeneratedStories(group, response);
            actions.Add(mediator.Send(new UpdateCommand<UserStoryGroupDomain> { Domain = group }, cancellationToken));

            // reset property SelectGroupId of the project
            ResetSelectedGroupOfTheProjectIfNeeded(project, group, actions, cancellationToken);

            // wait resolutions of the actions
            await Task.WhenAll(actions);

            return await GetByIdAsync(group.Id, cancellationToken) ?? throw new Exception($"Group '{group.Id}' not found after user stories generation");
        }
        #region GenerateUsertoriesAsync helpers

        private async Task<IEnumerable<DocumentDomain>> RehydrateDocuments(UserStoryGroupDomain domain, CancellationToken cancellationToken = default)
        {
            // upload files to the GenAI and store new Metadata
            var filter = new PropertyEqualsFilter(nameof(UserStoryGroupDomain.ProjectId), domain.ProjectId);
            IEnumerable<DocumentDomain> documents = await mediator.Send(new GetAllQuery<DocumentDomain>() { Filter = filter }, cancellationToken);
            if (!documents.Any()) throw new Exception("To generate user stories, at least one document is required");

            var uploads = await genAiAdapter.SendFilesAsync(documents, cancellationToken);
            await Task.WhenAll(uploads.Select(doc => mediator.Send(new UpdateCommand<DocumentDomain>() { Domain = doc }, cancellationToken)));

            return documents;
        }

        private async Task<string> SendRequestToGenAi(UserStoryGroupDomain domain, IEnumerable<DocumentDomain> documents, CancellationToken cancellationToken = default)
        {
            GeminiContent content = new();
            content.AddPart(ConvertRequestDomainToGeminiContent(domain.Request));
            if (documents is not null) content.AddParts(documents.Select(document => new GeminiContentPart(document.Metadata.MimeType, document.Metadata.Uri)));

            GeminiRequest data = new();
            data.AddContent(content);

            data.SetGenerationConfig(new()
            {
                MimeType = "application/json",
                Schema = resources.UserStoryRequestSchema ?? throw new Exception("User story schema for the result of the Gemini api is not found, check the resources.")
            });

            return await genAiAdapter.SendRequestAsync(data, cancellationToken);
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

        private static void CreateGeneratedStories(UserStoryGroupDomain domain, GeminiResponse response)
        {
            // check constraints
            var responseResult = response.Candidates.FirstOrDefault(i => i.Content.Role == "model")?.Content?.Parts.Last()
                ?? throw new InvalidOperationException("Error while getting the response text from the payload");

            var text = responseResult.Text
                ?? throw new InvalidOperationException("Error while getting the text result from the payload");

            // act: extract users stories from the GenAI response and set tasks costs
            var userStories = JsonSerializer.Deserialize<ICollection<UserStoryDomain>>(text) ?? [];
            foreach (var task in userStories.SelectMany(us => us.Tasks))
            {
                task.AddGeminiCost(task.Cost);
                task.Cost = 0;
            }
            // act: set user stories to the group and create them
            domain.SetUserStory(userStories);
        }

        private void ResetSelectedGroupOfTheProjectIfNeeded(ProjectDomain project, UserStoryGroupDomain domain, List<Task> actions, CancellationToken cancellationToken = default)
        {
            // when the selected group of the project is generated (or regenerated),
            // the selection must be unset

            if (project.SelectedGroupId is null || project.SelectedGroupId != domain.Id) return;

            project.SelectedGroupId = null;
            actions.Add(mediator.Send(new UpdateCommand<ProjectDomain> { Domain = project }, cancellationToken));
        }

        #endregion

        #endregion

        public async Task<UserStoryGroupDomain> ValidateCostsAsync(string projectId, string groupId, CancellationToken cancellationToken = default)
        {
            var project = await mediator.Send(new GetByIdQuery<ProjectDomain>() { Id = projectId }, cancellationToken) ?? throw new Exception("Project not found");
            var group = await mediator.Send(new GetByIdQuery<UserStoryGroupDomain>() { Id = groupId }, cancellationToken) ?? throw new Exception("Group not found");

            // revert previous selected group
            var selectedGroup = project.Groups.FirstOrDefault(x => x.Id == project.SelectedGroupId);
            if (selectedGroup is not null) ResetTaskCost(selectedGroup);

            // set cost for selected group
            project.SelectedGroupId = group.Id;
            OverrideWithDefaultCost(group);
            await UpdateAsync(group, cancellationToken);

            return (await GetByIdAsync(groupId, cancellationToken)) ?? throw new Exception("new item not found !");
        }
        #region ValidateCostsAsync helpers
        private static void ResetTaskCost(UserStoryGroupDomain item)
        {
            foreach (var story in item.UserStories)
                foreach (var task in story.Tasks)
                    task.Cost = 0;
        }
        private static void OverrideWithDefaultCost(UserStoryGroupDomain item)
        {
            foreach (var story in item.UserStories)
                foreach (var task in story.Tasks)
                {
                    // si on accepte par default la valeur de l'IA
                    var gemini = task.WorkingCosts.FirstOrDefault(cost => cost.Kind == TaskCostKind.Gemini);
                    if (task.Cost < 1 && gemini != null)
                        task.Cost = gemini.Cost;
                }
        }
        #endregion
    }
}
