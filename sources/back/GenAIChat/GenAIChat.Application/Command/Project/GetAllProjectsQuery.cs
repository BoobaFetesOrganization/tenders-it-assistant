using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Command.Project
{
    public class GetAllProjectsQueryHandler(IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<GetAllQuery<ProjectDomain>, IEnumerable<ProjectDomain>>
    {
        public async Task<IEnumerable<ProjectDomain>> Handle(GetAllQuery<ProjectDomain> request, CancellationToken cancellationToken)
            => await unitOfWork.Projects.GetAllAsync(request.Options, request.Filter);
    }
}
