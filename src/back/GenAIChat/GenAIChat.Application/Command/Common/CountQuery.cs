using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using MediatR;
using System.Linq.Expressions;

namespace GenAIChat.Application.Command.Common
{
    public class CountQuery<TDomain> : IRequest<CountQuery> where TDomain : class, IEntityDomain
    {
        public Expression<Func<TDomain, bool>>? Filter { get; init; }
    }
    public class CountQuery
    {
        public int Value { get; set; }
    }

    public class GetCountQueryHandler<TDomain>(IRepositoryAdapter<TDomain> repository) : IRequestHandler<CountQuery<TDomain>, CountQuery> where TDomain : class, IEntityDomain
    {
        public async Task<CountQuery> Handle(CountQuery<TDomain> request, CancellationToken cancellationToken)
            => new() { Value = await repository.CountAsync(request.Filter) };
    }
}
