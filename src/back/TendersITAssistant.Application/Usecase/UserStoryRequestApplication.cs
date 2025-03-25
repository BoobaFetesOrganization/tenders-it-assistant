using MediatR;
using Serilog;
using System.Text.Json;
using TendersITAssistant.Application.Command.Common;
using TendersITAssistant.Application.Usecase.Interface;
using TendersITAssistant.Domain.Project;
using TendersITAssistant.Domain.Project.Group;

namespace TendersITAssistant.Application.Usecase
{
    public class UserStoryRequestApplication(IMediator mediator, IUserStoryGroupApplication userStoryGroupApplication, ILogger logger) : ApplicationBase<UserStoryRequestDomain>(mediator, logger), IApplication<UserStoryRequestDomain>
    {
        public async override Task<bool> UpdateAsync(UserStoryRequestDomain domain, CancellationToken cancellationToken = default)
        {
            base.logger.Information("application - update - {0}", JsonSerializer.Serialize(domain));

            if (!await base.UpdateAsync(domain, cancellationToken)) return false;

            var group = await mediator.Send(new GetByIdQuery<UserStoryGroupDomain>() { Id = domain.GroupId }, cancellationToken)
                ?? throw new Exception($"Group '{domain.GroupId}' of the request '{domain.Id}' is not found");

            var project = await mediator.Send(new GetByIdQuery<ProjectDomain>() { Id = group.ProjectId }, cancellationToken)
                ?? throw new Exception($"Project '{group.ProjectId}' of the group '{group.Id}' is not found");

            base.logger.Information("application - update - {0} - generate usertories", JsonSerializer.Serialize(domain));
            await userStoryGroupApplication.GenerateUserStoriesAsync(project, group, cancellationToken);

            return true;
        }
    }
}
