using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project.Group.UserStory;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group.UserStory
{
    public class UserStoryDeleteCommandHandler(IGenAiUnitOfWorkAdapter unitOfWork) : IRequestHandler<DeleteCommand<UserStoryDomain>, UserStoryDomain?>
    {
        public async Task<UserStoryDomain?> Handle(DeleteCommand<UserStoryDomain> request, CancellationToken cancellationToken)
        {
            var item = await unitOfWork.UserStory.GetByIdAsync(request.Id);

            if (item is not null) await unitOfWork.UserStory.DeleteAsync(item);

            return item;
        }
    }

}
