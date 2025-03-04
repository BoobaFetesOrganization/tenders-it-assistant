using GenAIChat.Application.Command.Common;
using GenAIChat.Application.Usecase.Interface;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using MediatR;

namespace GenAIChat.Application.Usecase
{
    public class ApplicationBase<TDomain>(IMediator mediator) : IApplication<TDomain> where TDomain : EntityDomain, new()
    {
        protected readonly IMediator mediator = mediator;

        public async Task<Paged<TDomain>> GetAllPagedAsync(PaginationOptions options, IFilter? filter = null, CancellationToken cancellationToken = default)
            => await mediator.Send(new GetAllPagedQuery<TDomain> { Options = options, Filter = filter }, cancellationToken);

        public async Task<TDomain?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
            => await mediator.Send(new GetByIdQuery<TDomain> { Id = id }, cancellationToken);

        public async virtual Task<TDomain> CreateAsync(TDomain domain, CancellationToken cancellationToken = default)
            => await mediator.Send(new CreateCommand<TDomain> { Domain = domain }, cancellationToken);

        public async virtual Task<bool> UpdateAsync(TDomain domain, CancellationToken cancellationToken = default)
            => await mediator.Send(new UpdateCommand<TDomain> { Domain = domain }, cancellationToken);

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
            => await mediator.Send(new DeleteByIdCommand<TDomain> { Id = id }, cancellationToken);
    }
}
