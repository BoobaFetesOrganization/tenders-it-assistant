using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using MediatR;

namespace GenAIChat.Application.Command.Common
{
    public class DeleteByIdCommand<TDomain> : IRequest<TDomain?> where TDomain : class, IEntityDomain
    {
        public required string Id { get; init; }
    }

    public class GetDeleteByIdCommandHandler<TDomain>(IRepositoryAdapter<TDomain> repository, IMediator mediator) : IRequestHandler<DeleteByIdCommand<TDomain>, TDomain?> where TDomain : class, IEntityDomain
    {
        public async Task<TDomain?> Handle(DeleteByIdCommand<TDomain> request, CancellationToken cancellationToken)
            => await mediator.Send(new DeleteCommand<TDomain> { Entity = await repository.GetByIdAsync(request.Id) }, cancellationToken);
    }
}
