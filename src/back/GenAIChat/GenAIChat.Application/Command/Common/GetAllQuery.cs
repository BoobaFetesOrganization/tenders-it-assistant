using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using MediatR;
using System.Linq.Expressions;

namespace GenAIChat.Application.Command.Common
{
    public class GetAllQuery<TDomain> : IRequest<IEnumerable<TDomain>> where TDomain : class, IEntityDomain
    {
        public Expression<Func<TDomain, bool>>? Filter { get; init; }
    }

    public class GetAllQueryHandler<TDomain>(IRepositoryAdapter<TDomain> repository) : IRequestHandler<GetAllQuery<TDomain>, IEnumerable<TDomain>> where TDomain : class, IEntityDomain
    {
        public async Task<IEnumerable<TDomain>> Handle(GetAllQuery<TDomain> request, CancellationToken cancellationToken)
            => await repository.GetAllAsync(request.Filter);
    }
}
