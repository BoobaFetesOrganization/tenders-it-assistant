using TendersITAssistant.Domain.Common;
using MediatR;
using TendersITAssistant.Application.Adapter.Database;

namespace TendersITAssistant.Application.Command.Common
{
    public class DuplicateCommand<TDomain> : IRequest<TDomain> where TDomain : class, IEntityDomain
    {
        public required TDomain Domain { get; init; }
    }

    public class DuplicateCommanddHandler<TDomain>(IRepositoryAdapter<TDomain> repository) : IRequestHandler<DuplicateCommand<TDomain>, TDomain> where TDomain : class, IEntityDomain
    {
        public async Task<TDomain> Handle(DuplicateCommand<TDomain> request, CancellationToken cancellationToken = default)
        {
            // remove systematically the id and timestamp to allow to duplicate entity
            request.Domain.Id = string.Empty;
            request.Domain.Timestamp = null;

            return await repository.AddAsync(request.Domain, cancellationToken);
        }
    }
}
