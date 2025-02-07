using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Command.Project
{
    public class ProjectGetByIdQueryHandler(IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<GetByIdQuery<ProjectDomain>, ProjectDomain?>
    {
        public async Task<ProjectDomain?> Handle(GetByIdQuery<ProjectDomain> request, CancellationToken cancellationToken)
            => await unitOfWork.Project.GetByIdAsync(request.Id);
    }
}
