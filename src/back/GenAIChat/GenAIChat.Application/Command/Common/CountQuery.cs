using GenAIChat.Domain.Common;
using MediatR;
using System.Linq.Expressions;

namespace GenAIChat.Application.Command.Common
{
    public class CountQuery<TEntity> : IRequest<int> where TEntity : class, IEntityDomain
    {
        public Expression<Func<TEntity, bool>>? Filter { get; init; }
    }
}
