using TendersITAssistant.Domain.Common;
using MediatR;
using TendersITAssistant.Application.Adapter.Database;

namespace TendersITAssistant.Application.Command.Common
{
    public class CreateCommand<TDomain> : IRequest<TDomain> where TDomain : class, IEntityDomain
    {
        public required TDomain Domain { get; init; }
    }

    public class CreateCommandHandler<TDomain>(IRepositoryAdapter<TDomain> repository) : IRequestHandler<CreateCommand<TDomain>, TDomain> where TDomain : class, IEntityDomain
    {
        public async Task<TDomain> Handle(CreateCommand<TDomain> request, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(request.Domain.Id)) throw new Exception("Id should not be set to request a creation");
            if (request.Domain.Timestamp is not null) throw new Exception("Timestamp should not be set to request a creation");

            return await repository.AddAsync(request.Domain, cancellationToken);
        }
    }
}
