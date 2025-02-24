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
            var isExisting = (await userStoryRepository.GetAllAsync()).Any(p => p.Name.ToLower().Equals(request.Domain.Name.ToLower()));
            if (isExisting) throw new Exception("Name already exists");

            var item = await userStoryRepository.GetByIdAsync(request.Domain.Id);
            if (item is null) return null;

            item.Id = request.Domain.Id;
            item.Name = request.Domain.Name;
            item.Tasks = request.Domain.Tasks;

            await userStoryRepository.UpdateAsync(item);

            return item;
        }
    }

}
