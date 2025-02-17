using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project.Group.UserStory;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group.UserStory
{
    public class UserStoryCountQueryHandler(IRepositoryAdapter<UserStoryDomain> userStoryRepository) : IRequestHandler<CountQuery<UserStoryDomain>, int>
    {
        public async Task<int> Handle(CountQuery<UserStoryDomain> request, CancellationToken cancellationToken)
            => await userStoryRepository.CountAsync(request.Filter);
    }
}
