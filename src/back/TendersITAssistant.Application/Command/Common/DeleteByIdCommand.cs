using MediatR;
using Serilog;
using System.Text.Json;
using TendersITAssistant.Application.Extensions;
using TendersITAssistant.Domain.Common;

namespace TendersITAssistant.Application.Command.Common
{
    public class DeleteByIdCommand<TDomain> : IRequest<bool> where TDomain : class, IEntityDomain, new()
    {
        public required string Id { get; init; }
    }

    public class DeleteByIdCommandHandler<TDomain>(IMediator mediator, ILogger _logger) : IRequestHandler<DeleteByIdCommand<TDomain>, bool> where TDomain : class, IEntityDomain, new()
    {
        private readonly ILogger logger = _logger.ForCommandContext<TDomain>("DeleteById");

        public async Task<bool> Handle(DeleteByIdCommand<TDomain> request, CancellationToken cancellationToken = default)
        {
            logger.Information("command - delete by id - args - {0}", JsonSerializer.Serialize(request));

            var response = await mediator.Send(new DeleteCommand<TDomain> { Domain = new() { Id = request.Id } }, cancellationToken);

            logger.Information("command - delete by id - response - {0}", response);

            return response;
        }
    }
}
