using GenAIChat.Domain.Common;
using MediatR;

namespace GenAIChat.Application.Command.Common
{
    public class DeleteCommand<TEntity> : IRequest<TEntity?> where TEntity : class, IEntityDomain
    {
        public required string Id { get; init; }
    }
}
