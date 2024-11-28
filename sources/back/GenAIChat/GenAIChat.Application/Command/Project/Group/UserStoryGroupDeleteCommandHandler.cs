using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project.Group;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group
{
    public class UserStoryGroupDeleteCommandHandler(IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<DeleteCommand<UserStoryGroupDomain>, UserStoryGroupDomain?>
    {
        public async Task<UserStoryGroupDomain?> Handle(DeleteCommand<UserStoryGroupDomain> request, CancellationToken cancellationToken)
        {
            var item = await unitOfWork.UserStoryGroup.GetByIdAsync(request.Id);

            if (item is not null) await unitOfWork.UserStoryGroup.DeleteAsync(item);

            return item;
        }
    }

}
