using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Common;
using MediatR;
using System.Linq.Expressions;

namespace GenAIChat.Application.Usecase.Common
{
    public abstract class ApplicationBase<TDomain>(IMediator mediator) where TDomain : EntityDomain
    {
        public async Task<Paged<TDomain>> GetAllPagedAsync(PaginationOptions options, Expression<Func<TDomain, bool>>? filter = null)
            => await mediator.Send(new GetAllPagedQuery<TDomain> { Options = options, Filter = filter });

        public async Task<TDomain?> GetByIdAsync(string id)
            => await mediator.Send(new GetByIdQuery<TDomain> { Id = id });

        public async virtual Task<TDomain> CreateAsync(TDomain domain)
            => await mediator.Send(new CreateCommand<TDomain> { Domain = domain });

        public async virtual Task<TDomain?> UpdateAsync(TDomain domain)
            => await mediator.Send(new UpdateCommand<TDomain> { Domain = domain });

        public async Task<TDomain?> DeleteAsync(string id)
            => await mediator.Send(new DeleteByIdCommand<TDomain> { Id = id });
    }
}
