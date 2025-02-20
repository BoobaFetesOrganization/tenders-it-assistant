using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using MediatR;

namespace GenAIChat.Application.Command.Common
{
    public class GetByIdQuery<TEntity> : IRequest<TEntity?> where TEntity : class, IEntityDomain
    {
        public required string Id { get; init; }
    }

    public class GetByIdQueryHandler<TEntity>(IRepositoryAdapter<TEntity> repository) : IRequestHandler<GetByIdQuery<TEntity>, TEntity?> where TEntity : class, IEntityDomain
    {
        public async Task<TEntity?> Handle(GetByIdQuery<TEntity> request, CancellationToken cancellationToken)
            => await repository.GetByIdAsync(request.Id);
    }
}
