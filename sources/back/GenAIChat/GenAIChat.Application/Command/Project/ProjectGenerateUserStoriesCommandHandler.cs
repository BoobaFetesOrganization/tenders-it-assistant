using GenAIChat.Application.Adapter.Api;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Entity;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Command.Project
{
    public class ProjectGenerateUserStoriesCommand : IRequest<ProjectDomain>
    {
        public required int Id { get; init; }
    }

    public class ProjectGenerateUserStoriesCommandHandler(IGenAiApiAdapter genAiAdapter, IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<ProjectGenerateUserStoriesCommand, ProjectDomain>
    {
        public async Task<ProjectDomain> Handle(ProjectGenerateUserStoriesCommand request, CancellationToken cancellationToken)
        {
            var project = (await unitOfWork.Projects.GetByIdAsync(request.Id))
                 ?? throw new Exception("Project not found");

            // upload files to the GenAI
            var expiredDocuments = project.Documents.Where(d => d.Metadata.ExpirationTime < DateTime.Now);
            await genAiAdapter.SendFilesAsync(expiredDocuments);

            // send prompt to the GenAI
            project.PromptResponse = await genAiAdapter.SendPromptAsync(project.Prompt, project.Documents);

            // load user stories from the GenAI result
            project.SetUserStories(project.LoadUsersStoriesFromProject());

            await unitOfWork.Projects.UpdateAsync(project);

            return project;
        }
    }

}
