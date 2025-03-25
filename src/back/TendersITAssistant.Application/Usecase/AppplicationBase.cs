using MediatR;
using Serilog;
using System.Text.Json;
using TendersITAssistant.Application.Command.Common;
using TendersITAssistant.Application.Extensions;
using TendersITAssistant.Application.Usecase.Interface;
using TendersITAssistant.Domain.Common;
using TendersITAssistant.Domain.Filter;

namespace TendersITAssistant.Application.Usecase
{
    public class ApplicationBase<TDomain>(IMediator mediator, ILogger logger) : IApplication<TDomain> where TDomain : EntityDomain, new()
    {
        protected readonly IMediator mediator = mediator;
        protected readonly ILogger logger = logger.ForApplicationContext<TDomain>();

        public async Task<Paged<TDomain>> GetAllPagedAsync(PaginationOptions options, IFilter? filter = null, CancellationToken cancellationToken = default)
        {
            logger.Information("application - get all - args - {options} - {filter}", options, filter);

            var response = await mediator.Send(new GetAllPagedQuery<TDomain> { Options = options, Filter = filter }, cancellationToken);

            logger.Information("application - get all - response - {response}", response is not null ?JsonSerializer.Serialize(response):"null");

            return response;
        }

        public async Task<TDomain?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            logger.Information("application - get - {id}", id);

            var response = await mediator.Send(new GetByIdQuery<TDomain> { Id = id }, cancellationToken);

            logger.Information("application - get - response - {0}", response is not null ? JsonSerializer.Serialize(response) : "null");

            return response;
        }

        public async virtual Task<TDomain> CreateAsync(TDomain domain, CancellationToken cancellationToken = default)
        {
            logger.Information("application - create - args - {0}", JsonSerializer.Serialize(domain));

            var response = await mediator.Send(new CreateCommand<TDomain> { Domain = domain }, cancellationToken);

            logger.Information("application - create - response - {0}", JsonSerializer.Serialize(response));

            return response!;
        }

        public async virtual Task<bool> UpdateAsync(TDomain domain, CancellationToken cancellationToken = default)
        {
            logger.Information("application - update - args - {0}", JsonSerializer.Serialize(domain));

            var response = await mediator.Send(new UpdateCommand<TDomain> { Domain = domain }, cancellationToken);

            logger.Information("application - update - response - {0}", response);

            return response;
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            logger.Information("application - delete - {id}", id);

            var response = await mediator.Send(new DeleteByIdCommand<TDomain> { Id = id }, cancellationToken);

            logger.Information("application - delete - response - {0}", response);

            return response;
        }
    }
}
