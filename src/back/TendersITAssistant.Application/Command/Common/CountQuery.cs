using TendersITAssistant.Domain.Common;
using TendersITAssistant.Domain.Filter;
using MediatR;
using TendersITAssistant.Application.Adapter.Database;

namespace TendersITAssistant.Application.Command.Common
{
    public class CountQuery<TDomain> : IRequest<CountQuery> where TDomain : class, IEntityDomain
    {
        public IFilter? Filter { get; init; }
    }
    public class CountQuery
    {
        public int Value { get; set; }
    }

    public class CountQueryHandler<TDomain>(IRepositoryAdapter<TDomain> repository) : IRequestHandler<CountQuery<TDomain>, CountQuery> where TDomain : class, IEntityDomain
    {
        public async Task<CountQuery> Handle(CountQuery<TDomain> request, CancellationToken cancellationToken = default)
            => new() { Value = await repository.CountAsync(request.Filter, cancellationToken) };
    }
}
