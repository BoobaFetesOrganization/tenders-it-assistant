using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Project
{
    public class DeleteProjectCommand : IRequest<ProjectDomain?>
    {
        public readonly int Id;

        public DeleteProjectCommand(int id) => Id = id;
    }

    public class DeleteProjectCommandHandler(IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<DeleteProjectCommand, ProjectDomain?>
    {
        public async Task<ProjectDomain?> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await unitOfWork.Projects.GetByIdAsync(request.Id);

            if (project is not null) await unitOfWork.Projects.DeleteAsync(project);

            return project;
        }
    }

}
