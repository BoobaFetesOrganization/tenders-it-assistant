using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using MediatR;

namespace GenAIChat.Application.Command.Common
{
    public class CountQuery<TDomain> : IRequest<CountQuery> where TDomain : class, IEntityDomain
    {
        public IFilter? Filter { get; init; }
    }
    public class CountQuery
    {
        public int Value { get; set; }
    }

    public class GetCountQueryHandler<TDomain>(IRepositoryAdapter<TDomain> repository) : IRequestHandler<CountQuery<TDomain>, CountQuery> where TDomain : class, IEntityDomain
    {
        public async Task<CountQuery> Handle(CountQuery<TDomain> request, CancellationToken cancellationToken = default)
            => new() { Value = await repository.CountAsync(request.Filter) };
    }
}
