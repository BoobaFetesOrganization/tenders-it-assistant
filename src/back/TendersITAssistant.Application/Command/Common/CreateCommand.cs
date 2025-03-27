using MediatR;
using Serilog;
using System.Text.Json;
using TendersITAssistant.Application.Adapter.Database;
using TendersITAssistant.Application.Extensions;
using TendersITAssistant.Domain.Common;

namespace TendersITAssistant.Application.Command.Common
{
    public class CreateCommand<TDomain> : IRequest<TDomain> where TDomain : class, IEntityDomain
    {
        public required TDomain Domain { get; init; }
    }

    public class CreateCommandHandler<TDomain>(IRepositoryAdapter<TDomain> repository, ILogger _logger) : IRequestHandler<CreateCommand<TDomain>, TDomain> where TDomain : class, IEntityDomain
    {
                private readonly ILogger logger = _logger.ForCommandContext<TDomain>("Create");

        public async Task<TDomain> Handle(CreateCommand<TDomain> request, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(request.Domain.Id)) throw new Exception("Id should not be set to request a creation");
            if (request.Domain.Timestamp is not null) throw new Exception("Timestamp should not be set to request a creation");

            logger.Information("command - create - args - {0}", JsonSerializer.Serialize(request));

            var response = await repository.AddAsync(request.Domain, cancellationToken);

            logger.Information("command - create - response - {0}", response is not null ? JsonSerializer.Serialize(response) : "null");

            return response!;
        }
    }
}
