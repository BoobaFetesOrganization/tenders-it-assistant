using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Command.Project
{
    public class ProjectCountQueryHandler(IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<CountQuery<ProjectDomain>, int>
    {
        public async Task<int> Handle(CountQuery<ProjectDomain> request, CancellationToken cancellationToken)
            => await unitOfWork.Project.CountAsync(request.Filter);
    }
}
