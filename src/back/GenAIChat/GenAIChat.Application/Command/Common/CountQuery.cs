using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using MediatR;
using System.Linq.Expressions;

namespace GenAIChat.Application.Command.Common
{
    public class CountQuery<TEntity> : IRequest<CountQuery> where TEntity : class, IEntityDomain
    {
        public Expression<Func<TEntity, bool>>? Filter { get; init; }
    }
    public class CountQuery
    {
        public int Value { get; set; }
    }

    public class GetCountQueryHandler<TEntity>(IRepositoryAdapter<TEntity> repository) : IRequestHandler<CountQuery<TEntity>, CountQuery> where TEntity : class, IEntityDomain
    {
        public async Task<CountQuery> Handle(CountQuery<TEntity> request, CancellationToken cancellationToken)
            => new() { Value = await repository.CountAsync(request.Filter) };
    }
}
