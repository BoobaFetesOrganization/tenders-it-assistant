using TendersITAssistant.Domain.Common;
using MediatR;
using TendersITAssistant.Application.Adapter.Database;
using Serilog;
using TendersITAssistant.Application.Extensions;
using System.Text.Json;

namespace TendersITAssistant.Application.Command.Common
{
    public class UpdateCommand<TDomain> : IRequest<bool> where TDomain : class, IEntityDomain
    {
        public required TDomain Domain { get; init; }
    }

    public class UpdateCommandHandler<TDomain>(IRepositoryAdapter<TDomain> repository, ILogger _logger) : IRequestHandler<UpdateCommand<TDomain>, bool> where TDomain : class, IEntityDomain
    {
        private readonly ILogger logger = _logger.ForCommandContext<TDomain>("Update");

        public async Task<bool> Handle(UpdateCommand<TDomain> request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.Domain.Id)) throw new Exception("Id should be set to request an update");

            logger.Information("command - update - args - {0}", JsonSerializer.Serialize(request));

            var response =  await repository.UpdateAsync(request.Domain, cancellationToken);

            logger.Information("command - update - response - {0}", response);

            return response;
        }
    }
}
