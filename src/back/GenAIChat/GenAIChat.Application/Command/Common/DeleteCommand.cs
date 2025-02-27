using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using MediatR;

namespace GenAIChat.Application.Command.Common
{
    public class DeleteCommand<TDomain> : IRequest<TDomain?> where TDomain : class, IEntityDomain
    {
        public required TDomain? Domain { get; init; }
    }

    public class GetDeleteCommandHandler<TDomain>(IRepositoryAdapter<TDomain> repository) : IRequestHandler<DeleteCommand<TDomain>, TDomain?> where TDomain : class, IEntityDomain
    {
        public async Task<TDomain?> Handle(DeleteCommand<TDomain> request, CancellationToken cancellationToken)
        {
            if(request.Domain is null) throw new Exception("Domain is not set");



            var item = await repository.GetByIdAsync(request.Domain.Id);
            if (item is null) return null;
            return await repository.DeleteAsync(item);
        }
            => (request.Domain is null) ? null : await repository.DeleteAsync(request.Domain);
    }
}
