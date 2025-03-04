using GenAIChat.Application.Command.Common;
using GenAIChat.Application.Usecase.Interface;
using GenAIChat.Domain.Project;
using GenAIChat.Domain.Project.Group;
using MediatR;

namespace GenAIChat.Application.Usecase
{
    public class UserStoryRequestApplication(IMediator mediator, IUserStoryGroupApplication userStoryGroupApplication) : ApplicationBase<UserStoryRequestDomain>(mediator), IApplication<UserStoryRequestDomain>
    {
        public async override Task<bool?> UpdateAsync(UserStoryRequestDomain domain, CancellationToken cancellationToken = default)
        {
            await base.UpdateAsync(domain, cancellationToken);

            var group = await mediator.Send(new GetByIdQuery<UserStoryGroupDomain>() { Id = domain.GroupId }, cancellationToken)
                ?? throw new Exception($"Group '{domain.GroupId}' of the request '{domain.Id}' is not found");

            var project = await mediator.Send(new GetByIdQuery<ProjectDomain>() { Id = group.ProjectId }, cancellationToken)
                ?? throw new Exception($"Project '{group.ProjectId}' of the group '{group.Id}' is not found");

            await userStoryGroupApplication.GenerateUserStoriesAsync(project, group, cancellationToken);

            return true;
        }
    }
}
