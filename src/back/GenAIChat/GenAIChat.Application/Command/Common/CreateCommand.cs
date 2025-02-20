using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using MediatR;

namespace GenAIChat.Application.Command.Common
{
    public class CreateCommand<TEntity> : IRequest<TEntity> where TEntity : class, IEntityDomain, ICloneable
    {
        public required TEntity Entity { get; init; }
    }

    public class GetCreateCommandHandler<TEntity>(IRepositoryAdapter<TEntity> repository) : IRequestHandler<CreateCommand<TEntity>, TEntity> where TEntity : class, IEntityDomain, ICloneable
    {
        public async Task<TEntity> Handle(CreateCommand<TEntity> request, CancellationToken cancellationToken)
        {
            return await repository.AddAsync((TEntity)request.Entity.Clone());
        }
    }
}
