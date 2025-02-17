using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Command.Project
{
    public class ProjectCountQueryHandler(IRepositoryAdapter<ProjectDomain> projectRepository) : IRequestHandler<CountQuery<ProjectDomain>, int>
    {
        public async Task<int> Handle(CountQuery<ProjectDomain> request, CancellationToken cancellationToken)
            => await projectRepository.CountAsync(request.Filter);
    }
}
