using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using MediatR;
using System.Linq.Expressions;

namespace GenAIChat.Application.Command.Common
{
    public class GetAllQuery<TEntity> : IRequest<IEnumerable<TEntity>> where TEntity : class, IEntityDomain
    {
        public Expression<Func<TEntity, bool>>? Filter { get; init; }
    }

    public class GetAllQueryHandler<TEntity>(IRepositoryAdapter<TEntity> repository) : IRequestHandler<GetAllQuery<TEntity>, IEnumerable<TEntity>> where TEntity : class, IEntityDomain
    {
        public async Task<IEnumerable<TEntity>> Handle(GetAllQuery<TEntity> request, CancellationToken cancellationToken)
            => await repository.GetAllAsync(request.Filter);
    }
}
