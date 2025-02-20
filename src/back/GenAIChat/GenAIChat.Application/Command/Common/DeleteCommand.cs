using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using MediatR;

namespace GenAIChat.Application.Command.Common
{
    public class DeleteCommand<TEntity> : IRequest<TEntity?> where TEntity : class, IEntityDomain
    {
        public required TEntity? Entity { get; init; }
    }

    public class GetDeleteCommandHandler<TEntity>(IRepositoryAdapter<TEntity> repository) : IRequestHandler<DeleteCommand<TEntity>, TEntity?> where TEntity : class, IEntityDomain
    {
        public async Task<TEntity?> Handle(DeleteCommand<TEntity> request, CancellationToken cancellationToken)
            => (request.Entity is null) ? null : await repository.DeleteAsync(request.Entity);
    }
}
