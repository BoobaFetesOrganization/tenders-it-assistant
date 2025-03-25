using MediatR;
using Serilog;
using System.Text.Json;
using TendersITAssistant.Application.Adapter.Database;
using TendersITAssistant.Application.Extensions;
using TendersITAssistant.Domain.Common;
using TendersITAssistant.Domain.Filter;

namespace TendersITAssistant.Application.Command.Common
{
    public class GetAllPagedQuery<TDomain> : IRequest<Paged<TDomain>> where TDomain : class, IEntityDomain
    {
        public required PaginationOptions Options { get; init; }
        public IFilter? Filter { get; init; }
    }

    public class GetAllPagedQueryHandler<TDomain>(IRepositoryAdapter<TDomain> repository, ILogger _logger) : IRequestHandler<GetAllPagedQuery<TDomain>, Paged<TDomain>> where TDomain : class, IEntityDomain
    {
        private readonly ILogger logger = _logger.ForCommandContext<TDomain>("GetAllPaged");

        public async Task<Paged<TDomain>> Handle(GetAllPagedQuery<TDomain> request, CancellationToken cancellationToken = default)
        {
            logger.Information("command - get all paged - args - {0}", JsonSerializer.Serialize(request));

            var response = await repository.GetAllPagedAsync(request.Options, request.Filter, cancellationToken);

            logger.Information("command - get all paged - response - {0}", JsonSerializer.Serialize(response));

            return response!;
        }
    }
}
