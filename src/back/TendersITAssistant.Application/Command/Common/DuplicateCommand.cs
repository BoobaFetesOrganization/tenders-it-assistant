using MediatR;
using Serilog;
using System.Text.Json;
using TendersITAssistant.Application.Adapter.Database;
using TendersITAssistant.Application.Extensions;
using TendersITAssistant.Domain.Common;

namespace TendersITAssistant.Application.Command.Common
{
    public class DuplicateCommand<TDomain> : IRequest<TDomain> where TDomain : class, IEntityDomain
    {
        public required TDomain Domain { get; init; }
    }

    public class DuplicateCommanddHandler<TDomain>(IRepositoryAdapter<TDomain> repository, ILogger _logger) : IRequestHandler<DuplicateCommand<TDomain>, TDomain> where TDomain : class, IEntityDomain
    {
        private readonly ILogger logger = _logger.ForCommandContext<TDomain>("Duplicate");

        public async Task<TDomain> Handle(DuplicateCommand<TDomain> request, CancellationToken cancellationToken = default)
        {
            logger.Information("command - duplicate - args - {0}", JsonSerializer.Serialize(request));

            // remove systematically the id and timestamp to allow to duplicate entity
            request.Domain.Id = string.Empty;
            request.Domain.Timestamp = null;

            var response = await repository.AddAsync(request.Domain, cancellationToken);

            logger.Information("command - duplicate - response - {0}", response is not null ? JsonSerializer.Serialize(response) : "null");

            return response!;
        }
    }
}
