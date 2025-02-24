using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using MediatR;

namespace GenAIChat.Application.Command.Common
{
    public class UpdateCommand<TDomain> : IRequest<TDomain?> where TDomain : class, IEntityDomain
    {
        public required TDomain Entity { get; init; }
    }

    public class UpdateCommandHandler<TDomain>(IRepositoryAdapter<TDomain> repository) : IRequestHandler<UpdateCommand<TDomain>, TDomain?> where TDomain : class, IEntityDomain
    {
        public async Task<TDomain?> Handle(UpdateCommand<TDomain> request, CancellationToken cancellationToken)
            => await repository.UpdateAsync(request.Entity);
    }
}
