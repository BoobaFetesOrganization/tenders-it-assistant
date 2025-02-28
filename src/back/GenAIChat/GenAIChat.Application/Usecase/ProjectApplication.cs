using GenAIChat.Application.Command.Common;
using GenAIChat.Application.Usecase.Interface;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Usecase
{
#pragma warning disable CS9107 // Un paramètre est capturé dans l’état du type englobant et sa valeur est également passée au constructeur de base. La valeur peut également être capturée par la classe de base.
    public class ProjectApplication(IMediator mediator) : ApplicationBase<ProjectDomain>(mediator), IApplication<ProjectDomain>
#pragma warning restore CS9107 // Un paramètre est capturé dans l’état du type englobant et sa valeur est également passée au constructeur de base. La valeur peut également être capturée par la classe de base.
    {
        public async override Task<ProjectDomain> CreateAsync(ProjectDomain domain, CancellationToken cancellationToken = default)
        {
            await ThrowIfNameAlreadyExists(domain, cancellationToken);
            return await mediator.Send(new CreateCommand<ProjectDomain>() { Domain = domain }, cancellationToken);
        }

        public async override Task<bool?> UpdateAsync(ProjectDomain domain, CancellationToken cancellationToken = default)
        {
            await ThrowIfNameAlreadyExists(domain, cancellationToken);
            return await mediator.Send(new UpdateCommand<ProjectDomain>() { Domain = domain }, cancellationToken);
        }

        private async Task ThrowIfNameAlreadyExists(ProjectDomain domain, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(domain.Name)) throw new Exception("Name is required");

            var sameNames = await mediator.Send(new GetAllQuery<ProjectDomain>()
            {
                Filter = new PropertyEqualsFilter(nameof(ProjectDomain.Name), domain.Name)
            }, cancellationToken);
            if (sameNames.Any()) throw new Exception("Name already exists");
        }
    }
}
