using GenAIChat.Application.Adapter.Api;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Common;
using GenAIChat.Application.Entity;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Command.Project
{
    public class CreateProjectCommandHandler(IGenAiApiAdapter genAiAdapter, IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<CreateCommand<ProjectDomain>, ProjectDomain>
    {
        public async Task<ProjectDomain> Handle(CreateCommand<ProjectDomain> request, CancellationToken cancellationToken)
        {
            var isExisting = (await unitOfWork.Projects.GetAllAsync(PaginationOptions.All, p => p.Name.ToLower().Equals(request.Entity.Name.ToLower()))).Any();
            if (isExisting) throw new Exception("Project with the same name already exists");

            var project = new ProjectDomain(request.Entity.Name, request.Entity.Prompt, request.Entity.Documents);

            // upload files to the GenAI
            await genAiAdapter.SendFilesAsync(project.Documents);

            // send prompt to the GenAI
            project.PromptResponse = await genAiAdapter.SendPromptAsync(project.Prompt, project.Documents);

            // load user stories from the GenAI result
            project.SetUserStories(project.LoadUsersStoriesFromProject());

            await unitOfWork.Projects.AddAsync(project);

            return project;
        }
    }

}
