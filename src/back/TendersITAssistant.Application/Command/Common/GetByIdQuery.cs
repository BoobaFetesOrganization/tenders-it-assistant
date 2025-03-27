using MediatR;
using Serilog;
using System.Text.Json;
using TendersITAssistant.Application.Adapter.Database;
using TendersITAssistant.Application.Extensions;
using TendersITAssistant.Domain.Common;

namespace TendersITAssistant.Application.Command.Common
{
    public class GetByIdQuery<TDomain> : IRequest<TDomain?> where TDomain : class, IEntityDomain
    {
        public required string Id { get; init; }
    }

    public class GetByIdQueryHandler<TDomain>(IRepositoryAdapter<TDomain> repository, ILogger _logger) : IRequestHandler<GetByIdQuery<TDomain>, TDomain?> where TDomain : class, IEntityDomain
    {
        private readonly ILogger logger = _logger.ForCommandContext<TDomain>("GetById");

        public async Task<TDomain?> Handle(GetByIdQuery<TDomain> request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.Id)) throw new Exception("Id should be set to request an entity");

            logger.Information("command - get by id - args - {0}", JsonSerializer.Serialize(request));

            var response = await repository.GetByIdAsync(request.Id, cancellationToken);

            logger.Information("command - get by id - response - {0}", response is not null ? JsonSerializer.Serialize(response) : "null");

            return response;
        }
    }
}
