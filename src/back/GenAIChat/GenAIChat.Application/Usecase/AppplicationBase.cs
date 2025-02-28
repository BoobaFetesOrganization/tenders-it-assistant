using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using MediatR;

namespace GenAIChat.Application.Usecase
{
    public class ApplicationBase<TDomain> : IApplication<TDomain> where TDomain : EntityDomain, new()
    {
        protected readonly IMediator mediator;

#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur autre que Null lors de la fermeture du constructeur. Envisagez d’ajouter le modificateur « required » ou de déclarer le champ comme pouvant accepter la valeur Null.
        public ApplicationBase() { }
#pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur autre que Null lors de la fermeture du constructeur. Envisagez d’ajouter le modificateur « required » ou de déclarer le champ comme pouvant accepter la valeur Null.
        protected ApplicationBase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<Paged<TDomain>> GetAllPagedAsync(PaginationOptions options, IFilter? filter = null, CancellationToken cancellationToken = default)
            => await mediator.Send(new GetAllPagedQuery<TDomain> { Options = options, Filter = filter }, cancellationToken);

        public async Task<TDomain?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
            => await mediator.Send(new GetByIdQuery<TDomain> { Id = id }, cancellationToken);

        public async virtual Task<TDomain> CreateAsync(TDomain domain, CancellationToken cancellationToken = default)
            => await mediator.Send(new CreateCommand<TDomain> { Domain = domain }, cancellationToken);

        public async virtual Task<bool?> UpdateAsync(TDomain domain, CancellationToken cancellationToken = default)
            => await mediator.Send(new UpdateCommand<TDomain> { Domain = domain }, cancellationToken);

        public async Task<bool?> DeleteAsync(string id, CancellationToken cancellationToken = default)
            => await mediator.Send(new DeleteByIdCommand<TDomain> { Id = id }, cancellationToken);
    }
}
