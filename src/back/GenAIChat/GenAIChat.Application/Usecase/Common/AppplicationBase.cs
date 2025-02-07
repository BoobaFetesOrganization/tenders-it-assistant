using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Application.Common;
using GenAIChat.Domain.Common;
using MediatR;
using System.Linq.Expressions;

namespace GenAIChat.Application.Usecase.Common
{
    public abstract class ApplicationBase<TDomain>(IMediator mediator, IGenAiUnitOfWorkAdapter unitOfWork) where TDomain : class, IEntityDomain
    {
        public async Task<Paged<TDomain>> GetAllAsync(PaginationOptions options, Expression<Func<TDomain, bool>>? filter = null)
        {
            var count = await mediator.Send(new CountQuery<TDomain> { Filter = filter });
            var data = await mediator.Send(new GetAllQuery<TDomain> { Options = options, Filter = filter });
            return new Paged<TDomain>(options, count, data);
        }

        public async Task<TDomain?> GetByIdAsync(int id)
        {
            return await mediator.Send(new GetByIdQuery<TDomain> { Id = id });
        }

        public async virtual Task<TDomain> CreateAsync(TDomain entity)
        {
            var result = await mediator.Send(new CreateCommand<TDomain> { Entity = entity });
            await unitOfWork.SaveChangesAsync();
            return result;
        }

        public async virtual Task<TDomain?> UpdateAsync(TDomain entity)
        {
            var result = await mediator.Send(new UpdateCommand<TDomain> { Entity = entity });
            await unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<TDomain?> DeleteAsync(int id)
        {
            var result = await mediator.Send(new DeleteCommand<TDomain> { Id = id });
            await unitOfWork.SaveChangesAsync();
            return result;
        }
    }
}
