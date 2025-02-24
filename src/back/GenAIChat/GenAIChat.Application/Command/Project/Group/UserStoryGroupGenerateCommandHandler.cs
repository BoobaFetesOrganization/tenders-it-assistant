using GenAIChat.Application.Adapter.Api;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Gemini;
using GenAIChat.Domain.Gemini.GeminiCommon;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Domain.Project.Group.UserStory;
using MediatR;
using System.Text.Json;

namespace GenAIChat.Application.Command.Project.Group
{
    public class UserStoryGroupGenerateCommand : IRequest<UserStoryGroupDomain>
    {
        public required UserStoryGroupDomain Entity { get; init; }
    }

    public class UserStoryGroupGenerateCommandHandler(IGenAiApiAdapter genAiAdapter, IRepositoryAdapter<DocumentDomain> documentRepository) : IRequestHandler<UserStoryGroupGenerateCommand, UserStoryGroupDomain>
    {
        public const string ResultSchema = @"{
            ""type"":""array"",
            ""items"":{
                ""type"": ""object"",
                ""properties"": {
                    ""Id"": { ""type"": ""integer"" },
                    ""Name"": { ""type"": ""string"" },
                    ""Tasks"": {
                        ""type"": ""array"",
                        ""items"": {
                            ""type"": ""object"",
                            ""properties"": {
                                ""Id"": { ""type"": ""integer"" },
                                ""Name"": { ""type"": ""string"" },
                                ""Cost"": { ""type"": ""number"" }
                            },
                            ""required"": [""Id"", ""Name"", ""Cost""]
                        }
                    }
                },
                ""required"": [""Id"", ""Name"", ""Tasks""]
            }
        }";

        public async Task<UserStoryGroupDomain> Handle(UserStoryGroupGenerateCommand request, CancellationToken cancellationToken)
        {
            // upload files to the GenAI and store new Metadata
            IEnumerable<DocumentDomain> documents = await documentRepository.GetAllAsync(document => document.ProjectId == request.Entity.ProjectId);
            if (!documents.Any()) throw new Exception("To generate user stories, at least one document is required");

            var expiredDocuments = documents.Where(d => d.Metadata.ExpirationTime < DateTime.Now);
            await genAiAdapter.SendFilesAsync(
                expiredDocuments,
                async doc => await documentRepository.UpdateAsync(doc)
                );

            var updateActions = expiredDocuments.Select(doc => documentRepository.UpdateAsync(doc));
            await Task.WhenAll(updateActions);

            // send prompt to the GenAI
            GeminiRequest data = new();

            GeminiContent content = new();
            content.AddPart(new(request.Entity.Request.ToGenAIRequest()));
            if (documents is not null) content.AddParts(documents.Select(document => new GeminiContentPart(document.Metadata.MimeType, document.Metadata.Uri)));
            data.AddContent(content);

            data.SetGenerationConfig(GeminiGenerationConfig.AsJsonResult(ResultSchema));

            request.Entity.ClearUserStories();
            request.Entity.Response = await genAiAdapter.SendRequestAsync(data);

            var response = GeminiResponse.LoadFrom(request.Entity.Response);

            var responseResult = response.Candidates.FirstOrDefault(i => i.Content.Role == "model")?.Content?.Parts.Last()
                ?? throw new InvalidOperationException("Error while getting the response text from the payload");

            var text = responseResult.Text
                ?? throw new InvalidOperationException("Error while getting the text result from the payload");

            // load user stories from the GenAI result
            var temp = JsonSerializer.Deserialize<ICollection<UserStoryDomain>>(text) ?? [];

            throw new Exception("ARO : hsitoire pas claire !! à investiguer !");

            //var stories = new List<UserStoryDomain>(temp.Select(i => item.Clone()));
            //foreach (var item in 
            //{
            //    story.Id = EntityDomain.NewId();
            //    foreach (var task in story.Tasks)
            //    {
            //        task.Id = task.UserStoryId = EntityDomain.NewId();
            //        task.AddGeminiCost(task.Cost);
            //        task.Cost = 0;
            //    }
            //}
            // request.Entity.SetUserStory(stories);

            return request.Entity;
        }
    }

}
