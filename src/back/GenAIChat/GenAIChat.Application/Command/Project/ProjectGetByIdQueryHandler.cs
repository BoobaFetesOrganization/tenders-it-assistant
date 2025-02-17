using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Command.Project
{
    public class ProjectGetByIdQueryHandler(IRepositoryAdapter<ProjectDomain> projectRepository) : IRequestHandler<GetByIdQuery<ProjectDomain>, ProjectDomain?>
    {
        public async Task<ProjectDomain?> Handle(GetByIdQuery<ProjectDomain> request, CancellationToken cancellationToken)
            => await projectRepository.GetByIdAsync(request.Id);
    }
}
