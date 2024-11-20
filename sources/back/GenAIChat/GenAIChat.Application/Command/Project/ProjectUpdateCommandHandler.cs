using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Command.Project
{
    public class ProjectUpdateCommandHandler(IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<UpdateCommand<ProjectDomain>, ProjectDomain?>
    {
        public async Task<ProjectDomain?> Handle(UpdateCommand<ProjectDomain> request, CancellationToken cancellationToken)
        {
            var project = await unitOfWork.Projects.GetByIdAsync(request.Entity.Id);
            if (project is null) return null;

            project.Name = request.Entity.Name;
            project.Prompt = request.Entity.Prompt;

            await unitOfWork.Projects.UpdateAsync(project);

            return project;
        }
    }

}
