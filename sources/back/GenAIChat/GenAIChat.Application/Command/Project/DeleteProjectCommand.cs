using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Command.Project
{
    public class DeleteProjectCommandHandler(IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<DeleteCommand<ProjectDomain>, ProjectDomain?>
    {
        public async Task<ProjectDomain?> Handle(DeleteCommand<ProjectDomain> request, CancellationToken cancellationToken)
        {
            var project = await unitOfWork.Projects.GetByIdAsync(request.Id);

            if (project is not null) await unitOfWork.Projects.DeleteAsync(project);

            return project;
        }
    }

}
