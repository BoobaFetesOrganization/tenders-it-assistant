using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using MediatR;

namespace GenAIChat.Application.Command.Common
{
    public class DeleteCommand<TDomain> : IRequest<bool?> where TDomain : class, IEntityDomain
    {
        public required TDomain Domain { get; init; }
    }

    public class DeleteCommandHandler<TDomain>(IRepositoryAdapter<TDomain> repository) : IRequestHandler<DeleteCommand<TDomain>, bool?> where TDomain : class, IEntityDomain
    {
        public async Task<bool?> Handle(DeleteCommand<TDomain> request, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(request.Domain.Id)) throw new Exception("Id should be set to request a deletion");

            TDomain item = await repository.GetByIdAsync(request.Domain.Id, cancellationToken)
                ?? throw new Exception($"entity '{request.Domain.Id} of type '{nameof(TDomain)}' not found. The entity is not deleted.");

            return await repository.DeleteAsync(item, cancellationToken);
        }
    }
}
