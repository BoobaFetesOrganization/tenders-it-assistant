using GenAIChat.Domain.Common;
using MediatR;

namespace GenAIChat.Application.Command.Common
{
    public class UpdateCommand<TEntity> : IRequest<TEntity?> where TEntity : class, IEntityDomain
    {
        public required TEntity Entity { get; init; }
    }
}
