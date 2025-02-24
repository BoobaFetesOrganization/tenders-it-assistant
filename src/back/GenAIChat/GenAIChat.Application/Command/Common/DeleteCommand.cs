using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using MediatR;

namespace GenAIChat.Application.Command.Common
{
    public class DeleteCommand<TDomain> : IRequest<TDomain?> where TDomain : class, IEntityDomain
    {
        public required TDomain? Entity { get; init; }
    }

    public class GetDeleteCommandHandler<TDomain>(IRepositoryAdapter<TDomain> repository) : IRequestHandler<DeleteCommand<TDomain>, TDomain?> where TDomain : class, IEntityDomain
    {
        public async Task<TDomain?> Handle(DeleteCommand<TDomain> request, CancellationToken cancellationToken)
            => (request.Entity is null) ? null : await repository.DeleteAsync(request.Entity);
    }
}
