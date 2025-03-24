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
            logger.Information("get all - {options} - {filter}", options, filter);

            return await mediator.Send(new GetAllPagedQuery<TDomain> { Options = options, Filter = filter }, cancellationToken);
        }

        public async Task<TDomain?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            logger.Information("get - {id}", id);

            return await mediator.Send(new GetByIdQuery<TDomain> { Id = id }, cancellationToken);
        }

        public async virtual Task<TDomain> CreateAsync(TDomain domain, CancellationToken cancellationToken = default)
        {
            logger.Information("create - {0}", JsonSerializer.Serialize(domain));

            return await mediator.Send(new CreateCommand<TDomain> { Domain = domain }, cancellationToken);
        }

        public async virtual Task<bool> UpdateAsync(TDomain domain, CancellationToken cancellationToken = default)
        {
            logger.Information("update - {0}", JsonSerializer.Serialize(domain));

            return await mediator.Send(new UpdateCommand<TDomain> { Domain = domain }, cancellationToken);
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            logger.Information("delete - {id}", id);

            return await mediator.Send(new DeleteByIdCommand<TDomain> { Id = id }, cancellationToken);
        }
    }
}
