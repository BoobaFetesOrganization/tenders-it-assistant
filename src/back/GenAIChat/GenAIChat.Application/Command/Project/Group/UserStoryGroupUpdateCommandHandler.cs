using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project.Group;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group
{
    public class UserStoryGroupUpdateCommandHandler(IMediator mediator, IRepositoryAdapter<UserStoryGroupDomain> repository) : IRequestHandler<UpdateCommand<UserStoryGroupDomain>, UserStoryGroupDomain?>
    {
        public async Task<UserStoryGroupDomain?> Handle(UpdateCommand<UserStoryGroupDomain> request, CancellationToken cancellationToken)
        {
            var item = await repository.GetByIdAsync(request.Domain.Id);
            if (item is null) return null;

            await mediator.Send(new UpdateCommand<UserStoryRequestDomain>()
            {
                Domain = request.Domain.Request
            }, cancellationToken);

            item.Response = request.Domain.Response;
            item.ProjectId = request.Domain.ProjectId;

            return await repository.UpdateAsync(item);
        }
    }

}
