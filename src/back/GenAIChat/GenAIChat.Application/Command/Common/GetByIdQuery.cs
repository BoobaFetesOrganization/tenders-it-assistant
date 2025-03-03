using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using MediatR;

namespace GenAIChat.Application.Command.Common
{
    public class GetByIdQuery<TDomain> : IRequest<TDomain?> where TDomain : class, IEntityDomain
    {
        public required string Id { get; init; }
    }

    public class GetByIdQueryHandler<TDomain>(IRepositoryAdapter<TDomain> repository) : IRequestHandler<GetByIdQuery<TDomain>, TDomain?> where TDomain : class, IEntityDomain
    {
        public async Task<TDomain?> Handle(GetByIdQuery<TDomain> request, CancellationToken cancellationToken = default)
            => await repository.GetByIdAsync(request.Id, cancellationToken);
    }
}
