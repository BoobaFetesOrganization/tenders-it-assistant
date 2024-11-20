using GenAIChat.Application.Adapter.Api;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Application.Entity;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Command.Project
{
    public class ProjectUpdateCommandHandler(IGenAiApiAdapter genAiAdapter, IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<UpdateCommand<ProjectDomain>, ProjectDomain?>
    {
        public async Task<ProjectDomain?> Handle(UpdateCommand<ProjectDomain> request, CancellationToken cancellationToken)
        {
            var project = await unitOfWork.Projects.GetByIdAsync(request.Entity.Id);
            if (project is null) return null;

            project.Name = request.Entity.Name;
            project.Prompt = request.Entity.Prompt;

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
