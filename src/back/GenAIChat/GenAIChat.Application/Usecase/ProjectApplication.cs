using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project;
using MediatR;

namespace GenAIChat.Application.Usecase
{
    public class ProjectApplication : ApplicationBase<ProjectDomain>, IApplication<ProjectDomain>
    {
        public ProjectApplication(IMediator mediator) : base(mediator) { }

        public async override Task<ProjectDomain> CreateAsync(ProjectDomain domain, CancellationToken cancellationToken)
        {
            ThrowIfNameAlreadyExists(domain, cancellationToken);
            return await mediator.Send(new CreateCommand<ProjectDomain>() { Domain = domain }, cancellationToken);
        }

        public async override Task<bool?> UpdateAsync(ProjectDomain domain, CancellationToken cancellationToken)
        {
            ThrowIfNameAlreadyExists(domain, cancellationToken);
            return await mediator.Send(new UpdateCommand<ProjectDomain>() { Domain = domain }, cancellationToken);
        }

        private async void ThrowIfNameAlreadyExists(ProjectDomain domain, CancellationToken cancellationToken)
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
