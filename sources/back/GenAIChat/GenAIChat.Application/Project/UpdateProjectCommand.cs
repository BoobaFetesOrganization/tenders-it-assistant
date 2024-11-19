using GenAIChat.Application.Adapter.Api;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Project
{
    public class UpdateProjectCommand : IRequest<ProjectDomain?>
    {
        public readonly ProjectDomain Project;

        public UpdateProjectCommand(ProjectDomain project) => Project = project;
    }

    public class UpdateProjectCommandHandler(IGenAiApiAdapter genAiAdapter, IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<UpdateProjectCommand, ProjectDomain?>
    {

        public async Task<ProjectDomain?> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await unitOfWork.Projects.GetByIdAsync(request.Project.Id);
            if (project is null) return null;

            project.Name = request.Project.Name;
            project.Prompt = request.Project.Prompt;

            // upload files to the GenAI
            var expiredDocuments = project.Documents.Where(d => d.Metadata.ExpirationTime < DateTime.Now);
            await genAiAdapter.SendFilesAsync(expiredDocuments);

            // send prompt to the GenAI
            project.PromptResponse = await genAiAdapter.SendPromptAsync(project.Prompt, project.Documents);

            // load user stories from the GenAI result
            IEnumerable<UserStoryDomain> userstories = project.LoadUsersStoriesFromProject();
            project.SetUserStories(userstories);

            await unitOfWork.Projects.UpdateAsync(project);

            return project;
        }
    }

}
