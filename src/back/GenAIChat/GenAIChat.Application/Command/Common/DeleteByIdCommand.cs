using GenAIChat.Domain.Common;
using MediatR;

namespace GenAIChat.Application.Command.Common
{
    public class DeleteByIdCommand<TDomain> : IRequest<bool> where TDomain : class, IEntityDomain, new()
    {
        public required string Id { get; init; }
    }

    public class DeleteByIdCommandHandler<TDomain>(IMediator mediator) : IRequestHandler<DeleteByIdCommand<TDomain>, bool> where TDomain : class, IEntityDomain, new()
    {
        public async Task<bool> Handle(DeleteByIdCommand<TDomain> request, CancellationToken cancellationToken = default)
            => await mediator.Send(new DeleteCommand<TDomain> { Domain = new() { Id = request.Id } }, cancellationToken);
    }
}
