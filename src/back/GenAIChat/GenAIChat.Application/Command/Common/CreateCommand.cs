using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using MediatR;

namespace GenAIChat.Application.Command.Common
{
    public class CreateCommand<TDomain> : IRequest<TDomain> where TDomain : class, IEntityDomain
    {
        public required TDomain Domain { get; init; }
    }

    public class GetCreateCommandHandler<TDomain>(IRepositoryAdapter<TDomain> repository) : IRequestHandler<CreateCommand<TDomain>, TDomain> where TDomain : class, IEntityDomain
    {
        public async Task<TDomain> Handle(CreateCommand<TDomain> request, CancellationToken cancellationToken = default)
            => await repository.AddAsync(request.Domain, cancellationToken);
    }
}
