using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project.Group.UserStory;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group.UserStory
{
    public class UserStoryUpdateCommandHandler(IRepositoryAdapter<UserStoryDomain> userStoryRepository) : IRequestHandler<UpdateCommand<UserStoryDomain>, UserStoryDomain?>
    {
        public async Task<UserStoryDomain?> Handle(UpdateCommand<UserStoryDomain> request, CancellationToken cancellationToken)
        {
            var filter = new PropertyEqualsFilter(nameof(UserStoryDomain.Name), request.Domain.Name);
            var sameNames = await userStoryRepository.GetAllAsync(filter);
            if (sameNames.Any()) throw new Exception("Name already exists");

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
