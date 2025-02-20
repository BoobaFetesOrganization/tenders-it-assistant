using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using MediatR;
using System.Linq.Expressions;

namespace GenAIChat.Application.Command.Common
{
    public class GetAllPagedQuery<TEntity> : IRequest<Paged<TEntity>> where TEntity : class, IEntityDomain
    {
        public required PaginationOptions Options { get; init; }
        public Expression<Func<TEntity, bool>>? Filter { get; init; }
    }

    public class GetAllPagedQueryHandler<TEntity>(IRepositoryAdapter<TEntity> repository) : IRequestHandler<GetAllPagedQuery<TEntity>, Paged<TEntity>> where TEntity : class, IEntityDomain
    {
        public async Task<Paged<TEntity>> Handle(GetAllPagedQuery<TEntity> request, CancellationToken cancellationToken)
            => await repository.GetAllPagedAsync(request.Options, request.Filter);
    }
}
