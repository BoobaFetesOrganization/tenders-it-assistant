using GenAIChat.Domain.Common;
using MediatR;

namespace GenAIChat.Application.Command.Common
{
    public class GetByIdQuery<TEntity> : IRequest<TEntity?> where TEntity : class, IEntityDomain
    {
        public required int Id { get; init; }
    }
}
