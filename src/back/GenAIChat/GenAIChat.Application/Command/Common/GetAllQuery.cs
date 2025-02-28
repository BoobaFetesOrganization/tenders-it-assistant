using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using MediatR;

namespace GenAIChat.Application.Command.Common
{
    public class GetAllQuery<TDomain> : IRequest<IEnumerable<TDomain>> where TDomain : class, IEntityDomain
    {
        public IFilter? Filter { get; init; }
    }

    public class GetAllQueryHandler<TDomain>(IRepositoryAdapter<TDomain> repository) : IRequestHandler<GetAllQuery<TDomain>, IEnumerable<TDomain>> where TDomain : class, IEntityDomain
    {
        public async Task<IEnumerable<TDomain>> Handle(GetAllQuery<TDomain> request, CancellationToken cancellationToken = default)
            => await repository.GetAllAsync(request.Filter);
    }
}
