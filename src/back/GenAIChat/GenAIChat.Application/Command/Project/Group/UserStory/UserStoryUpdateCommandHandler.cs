using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Project.Group.UserStory;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group.UserStory
{
    public class UserStoryUpdateCommandHandler(IRepositoryAdapter<UserStoryDomain> userStoryRepository) : IRequestHandler<UpdateCommand<UserStoryDomain>, UserStoryDomain?>
    {
        public async Task<UserStoryDomain?> Handle(UpdateCommand<UserStoryDomain> request, CancellationToken cancellationToken)
        {
            var isExisting = (await userStoryRepository.GetAllAsync(PaginationOptions.All, p => p.Name.ToLower().Equals(request.Entity.Name.ToLower()))).Any();
            if (isExisting) throw new Exception("Name already exists");

            var item = await userStoryRepository.GetByIdAsync(request.Entity.Id);
            if (item is null) return null;

            item.Id = request.Entity.Id;
            item.Name = request.Entity.Name;
            item.Tasks = request.Entity.Tasks;

            await userStoryRepository.UpdateAsync(item);

            return item;
        }
    }

}
