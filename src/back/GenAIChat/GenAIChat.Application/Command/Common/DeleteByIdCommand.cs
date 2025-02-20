using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using MediatR;

namespace GenAIChat.Application.Command.Common
{
    public class DeleteByIdCommand<TEntity> : IRequest<TEntity?> where TEntity : class, IEntityDomain
    {
        public required string Id { get; init; }
    }

    public class GetDeleteByIdCommandHandler<TEntity>(IRepositoryAdapter<TEntity> repository, IMediator mediator) : IRequestHandler<DeleteByIdCommand<TEntity>, TEntity?> where TEntity : class, IEntityDomain
    {
        public async Task<TEntity?> Handle(DeleteByIdCommand<TEntity> request, CancellationToken cancellationToken)
            => await mediator.Send(new DeleteCommand<TEntity> { Entity = await repository.GetByIdAsync(request.Id) }, cancellationToken);
    }
}
