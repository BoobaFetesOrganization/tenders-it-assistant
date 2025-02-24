using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using MediatR;
using System.Linq.Expressions;

namespace GenAIChat.Application.Command.Common
{
    public class GetAllPagedQuery<TDomain> : IRequest<Paged<TDomain>> where TDomain : class, IEntityDomain
    {
        public required PaginationOptions Options { get; init; }
        public Expression<Func<TDomain, bool>>? Filter { get; init; }
    }

    public class GetAllPagedQueryHandler<TDomain>(IRepositoryAdapter<TDomain> repository) : IRequestHandler<GetAllPagedQuery<TDomain>, Paged<TDomain>> where TDomain : class, IEntityDomain
    {
        public async Task<Paged<TDomain>> Handle(GetAllPagedQuery<TDomain> request, CancellationToken cancellationToken)
            => await repository.GetAllPagedAsync(request.Options, request.Filter);
    }
}
