using TendersITAssistant.Domain.Common;
using TendersITAssistant.Domain.Filter;
using MediatR;
using TendersITAssistant.Application.Adapter.Database;
using Serilog;
using TendersITAssistant.Application.Extensions;
using System.Text.Json;

namespace TendersITAssistant.Application.Command.Common
{
    public class GetAllQuery<TDomain> : IRequest<IEnumerable<TDomain>> where TDomain : class, IEntityDomain
    {
        public IFilter? Filter { get; init; }
    }

    public class GetAllQueryHandler<TDomain>(IRepositoryAdapter<TDomain> repository, ILogger _logger) : IRequestHandler<GetAllQuery<TDomain>, IEnumerable<TDomain>> where TDomain : class, IEntityDomain
    {
        private readonly ILogger logger = _logger.ForCommandContext<TDomain>("Create");

        public async Task<IEnumerable<TDomain>> Handle(GetAllQuery<TDomain> request, CancellationToken cancellationToken = default)
        {
            logger.Information("command - get all - args - {0}", JsonSerializer.Serialize(request));

            var response = await repository.GetAllAsync(request.Filter, cancellationToken);

            logger.Information("command - get all - response - {0}", JsonSerializer.Serialize(response));

            return response;
        }
    }
}
