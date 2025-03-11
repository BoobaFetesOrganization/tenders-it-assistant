using MediatR;
using Serilog;
using TendersITAssistant.Application.Command.Common;
using TendersITAssistant.Application.Usecase.Interface;
using TendersITAssistant.Domain.Filter;
using TendersITAssistant.Domain.Project;

namespace TendersITAssistant.Application.Usecase
{
    public class ProjectApplication(IMediator mediator, ILogger logger) : ApplicationBase<ProjectDomain>(mediator, logger), IApplication<ProjectDomain>
    {
        public async override Task<ProjectDomain> CreateAsync(ProjectDomain domain, CancellationToken cancellationToken = default)
        {
            await ThrowIfNameAlreadyExists(domain, cancellationToken);
            return await mediator.Send(new CreateCommand<ProjectDomain>() { Domain = domain }, cancellationToken);
        }

        public async override Task<bool> UpdateAsync(ProjectDomain domain, CancellationToken cancellationToken = default)
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
