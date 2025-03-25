using MediatR;
using Serilog;
using System.Text.Json;
using TendersITAssistant.Application.Adapter.Database;
using TendersITAssistant.Application.Extensions;
using TendersITAssistant.Domain.Common;

namespace TendersITAssistant.Application.Command.Common
{
    public class DeleteCommand<TDomain> : IRequest<bool> where TDomain : class, IEntityDomain
    {
        public required TDomain Domain { get; init; }
    }

    public class DeleteCommandHandler<TDomain>(IRepositoryAdapter<TDomain> repository, ILogger _logger) : IRequestHandler<DeleteCommand<TDomain>, bool> where TDomain : class, IEntityDomain
    {
        private readonly ILogger logger = _logger.ForCommandContext<TDomain>("Delete");

        public async Task<bool> Handle(DeleteCommand<TDomain> request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.Domain.Id)) throw new Exception("Id should be set to request a deletion");

            logger.Information("command - delete - args - {0}", JsonSerializer.Serialize(request));

            TDomain item = await repository.GetByIdAsync(request.Domain.Id, cancellationToken)
                ?? throw new Exception($"entity '{request.Domain.Id} of type '{nameof(TDomain)}' not found. The entity is not deleted.");

            var response = await repository.DeleteAsync(item, cancellationToken);

            logger.Information("command - delete - response - {0}", response);

            return response;
        }
    }
}
