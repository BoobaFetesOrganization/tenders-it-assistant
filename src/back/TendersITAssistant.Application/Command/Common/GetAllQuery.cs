using TendersITAssistant.Domain.Common;
using TendersITAssistant.Domain.Filter;
using MediatR;
using TendersITAssistant.Application.Adapter.Database;

namespace TendersITAssistant.Application.Command.Common
{
    public class GetAllQuery<TDomain> : IRequest<IEnumerable<TDomain>> where TDomain : class, IEntityDomain
    {
        public IFilter? Filter { get; init; }
    }

    public class GetAllQueryHandler<TDomain>(IRepositoryAdapter<TDomain> repository) : IRequestHandler<GetAllQuery<TDomain>, IEnumerable<TDomain>> where TDomain : class, IEntityDomain
    {
        public async Task<IEnumerable<TDomain>> Handle(GetAllQuery<TDomain> request, CancellationToken cancellationToken = default)
            => await repository.GetAllAsync(request.Filter, cancellationToken);
    }
}
