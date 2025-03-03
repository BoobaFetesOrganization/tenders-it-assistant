using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using MediatR;

namespace GenAIChat.Application.Command.Common
{
    public class GetAllPagedQuery<TDomain> : IRequest<Paged<TDomain>> where TDomain : class, IEntityDomain
    {
        public required PaginationOptions Options { get; init; }
        public IFilter? Filter { get; init; }
    }

    public class GetAllPagedQueryHandler<TDomain>(IRepositoryAdapter<TDomain> repository) : IRequestHandler<GetAllPagedQuery<TDomain>, Paged<TDomain>> where TDomain : class, IEntityDomain
    {
        public async Task<Paged<TDomain>> Handle(GetAllPagedQuery<TDomain> request, CancellationToken cancellationToken = default)
            => await repository.GetAllPagedAsync(request.Options, request.Filter, cancellationToken);
    }
}
