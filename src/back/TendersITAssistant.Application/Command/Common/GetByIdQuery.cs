using TendersITAssistant.Domain.Common;
using MediatR;
using TendersITAssistant.Application.Adapter.Database;

namespace TendersITAssistant.Application.Command.Common
{
    public class GetByIdQuery<TDomain> : IRequest<TDomain?> where TDomain : class, IEntityDomain
    {
        public required string Id { get; init; }
    }

    public class GetByIdQueryHandler<TDomain>(IRepositoryAdapter<TDomain> repository) : IRequestHandler<GetByIdQuery<TDomain>, TDomain?> where TDomain : class, IEntityDomain
    {
        public async Task<TDomain?> Handle(GetByIdQuery<TDomain> request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.Id)) throw new Exception("Id should be set to request an entity");

            return await repository.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}
