using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project.Group.UserStory;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group.UserStory
{
    public class UserStoryDeleteCommandHandler(IRepositoryAdapter<UserStoryDomain> userStoryRepository) : IRequestHandler<DeleteCommand<UserStoryDomain>, UserStoryDomain?>
    {
        public async Task<UserStoryDomain?> Handle(DeleteCommand<UserStoryDomain> request, CancellationToken cancellationToken)
        {
            var item = await userStoryRepository.GetByIdAsync(request.Id);

            if (item is not null) await userStoryRepository.DeleteAsync(item);

            return item;
        }
    }

}
