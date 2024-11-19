using GenAIChat.Application.Adapter.Api;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Project
{
    public class CreateProjectCommand : IRequest<ProjectDomain>
    {
        public readonly string Name;
        public readonly string Prompt;
        public readonly IEnumerable<DocumentDomain>? Documents;

        public CreateProjectCommand(string name, string prompt, IEnumerable<DocumentDomain>? documents = null)
        {
            Name = name;
            Prompt = prompt;
            Documents = documents;
        }
    }

    public class CreateProjectCommandHandler(IGenAiApiAdapter genAiAdapter, IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<CreateProjectCommand, ProjectDomain>
    {
        public async Task<ProjectDomain> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var isExisting = (await unitOfWork.Projects.GetAllAsync(PaginationOptions.All, p => p.Name.ToLower().Equals(request.Name.ToLower()))).Any();
            if (isExisting)
            {
                throw new Exception("Project with the same name already exists");
            }

            var project = new ProjectDomain(request.Name, request.Prompt);

            // upload files to the GenAI
            if (request.Documents is not null)
            {
                project.SetDocuments(request.Documents);
                await genAiAdapter.SendFilesAsync(request.Documents);
            }

            // send prompt to the GenAI
            project.PromptResponse = await genAiAdapter.SendPromptAsync(request.Prompt, request.Documents);

            // load user stories from the GenAI result
            IEnumerable<UserStoryDomain> userstories = project.LoadUsersStoriesFromProject();
            project.SetUserStories(userstories);

            await unitOfWork.Projects.AddAsync(project);

            return project;
        }
    }

}
