using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Command.Project
{
    public class ProjectGetAllQueryHandler(IRepositoryAdapter<ProjectDomain> projectRepository) : IRequestHandler<GetAllQuery<ProjectDomain>, IEnumerable<ProjectDomain>>
    {
        public async Task<IEnumerable<ProjectDomain>> Handle(GetAllQuery<ProjectDomain> request, CancellationToken cancellationToken)
            => await projectRepository.GetAllAsync(request.Options, request.Filter);
    }
}
