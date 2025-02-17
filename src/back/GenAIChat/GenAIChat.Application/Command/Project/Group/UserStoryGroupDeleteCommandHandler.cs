using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project.Group;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group
{
    public class UserStoryGroupDeleteCommandHandler(IRepositoryAdapter<UserStoryGroupDomain> userStoryGroupRepository) : IRequestHandler<DeleteCommand<UserStoryGroupDomain>, UserStoryGroupDomain?>
    {
        public async Task<UserStoryGroupDomain?> Handle(DeleteCommand<UserStoryGroupDomain> request, CancellationToken cancellationToken)
        {
            var item = await userStoryGroupRepository.GetByIdAsync(request.Id);

            if (item is not null) await userStoryGroupRepository.DeleteAsync(item);

            return item;
        }
    }

}
