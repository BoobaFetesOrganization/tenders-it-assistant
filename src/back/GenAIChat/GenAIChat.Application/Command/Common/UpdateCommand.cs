using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using MediatR;

namespace GenAIChat.Application.Command.Common
{
    public class UpdateCommand<TEntity> : IRequest<TEntity?> where TEntity : class, IEntityDomain
    {
        public required TEntity Entity { get; init; }
    }

    public class UpdateCommandHandler<TEntity>(IRepositoryAdapter<TEntity> repository) : IRequestHandler<UpdateCommand<TEntity>, TEntity?> where TEntity : class, IEntityDomain
    {
        public async Task<TEntity?> Handle(UpdateCommand<TEntity> request, CancellationToken cancellationToken)
            => await repository.UpdateAsync(request.Entity);
    }
}
