using GenAIChat.Domain.Common;
using MediatR;
using System.Linq.Expressions;

namespace GenAIChat.Application.Command.Common
{
    public class GetAllQuery<TEntity> : IRequest<IEnumerable<TEntity>> where TEntity : class, IEntityDomain
    {
        public required PaginationOptions Options { get; init; }
        public Expression<Func<TEntity, bool>>? Filter { get; init; }
    }
}
