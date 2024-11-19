using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Project;
using MediatR;
using System.Linq.Expressions;

namespace GenAIChat.Application.Project
{
    public class GetAllProjectsQuery : IRequest<IEnumerable<ProjectDomain>>
    {
        public readonly PaginationOptions Options;
        public readonly Expression<Func<ProjectDomain, bool>>? Filter;

        public GetAllProjectsQuery(PaginationOptions options, Expression<Func<ProjectDomain, bool>>? filter = null)
        {
            Options = options;
            Filter = filter;
        }
    }

    public class GetAllProjectsQueryHandler(IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<GetAllProjectsQuery, IEnumerable<ProjectDomain>>
    {
        public async Task<IEnumerable<ProjectDomain>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            return await unitOfWork.Projects.GetAllAsync(request.Options, request.Filter);
        }
    }
}
