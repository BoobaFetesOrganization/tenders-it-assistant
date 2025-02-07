using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project.Group;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group
{
    public class UserStoryGroupUpdateCommandHandler(IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<UpdateCommand<UserStoryGroupDomain>, UserStoryGroupDomain?>
    {
        public async Task<UserStoryGroupDomain?> Handle(UpdateCommand<UserStoryGroupDomain> request, CancellationToken cancellationToken)
        {
            var item = await unitOfWork.UserStoryGroup.GetByIdAsync(request.Entity.Id);
            if (item is null) return null;

            item.Request= request.Entity.Request;
            item.Response = request.Entity.Response;
            item.ProjectId = request.Entity.ProjectId;
            item.UserStories = request.Entity.UserStories;

            await unitOfWork.UserStoryGroup.UpdateAsync(item);

            return item;
        }
    }

}
