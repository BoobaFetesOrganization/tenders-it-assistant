using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Common;
using MediatR;
using System.Linq.Expressions;

namespace GenAIChat.Application.Usecase.Common
{
    public abstract class ApplicationBase<TDomain> where TDomain : class, IEntityDomain
    {
        protected readonly IMediator mediator;
        protected readonly IRepositoryAdapter<TDomain> repository;

        protected ApplicationBase(IMediator mediator, IRepositoryAdapter<TDomain> repository)
        {
            this.mediator = mediator;
            this.repository = repository;
        }

        public async Task<Paged<TDomain>> GetAllAsync(PaginationOptions options, Expression<Func<TDomain, bool>>? filter = null)
        {
            var count = await mediator.Send(new CountQuery<TDomain> { Filter = filter });
            var data = await mediator.Send(new GetAllQuery<TDomain> { Options = options, Filter = filter });
            return new Paged<TDomain>(options, count, data);
        }

        public async Task<TDomain?> GetByIdAsync(string id)
        {
            return await mediator.Send(new GetByIdQuery<TDomain> { Id = id });
        }

        public async virtual Task<TDomain> CreateAsync(TDomain entity)
        {
            var result = await mediator.Send(new CreateCommand<TDomain> { Entity = entity });
            await repository.SaveAsync();
            return result;
        }

        public async virtual Task<TDomain?> UpdateAsync(TDomain entity)
        {
            var result = await mediator.Send(new UpdateCommand<TDomain> { Entity = entity });
            await repository.SaveAsync();
            return result;
        }

        public async Task<TDomain?> DeleteAsync(string id)
        {
            var result = await mediator.Send(new DeleteCommand<TDomain> { Id = id });
            await repository.SaveAsync();
            return result;
        }
    }
}
