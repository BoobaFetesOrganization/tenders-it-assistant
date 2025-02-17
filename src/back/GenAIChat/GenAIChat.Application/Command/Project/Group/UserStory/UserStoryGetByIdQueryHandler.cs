using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Command.Common;
using GenAIChat.Domain.Project.Group.UserStory;
using MediatR;

namespace GenAIChat.Application.Command.Project.Group.UserStory
{
    public class UserStoryGetByIdQueryHandler(IRepositoryAdapter<UserStoryDomain> userStoryRepository) : IRequestHandler<GetByIdQuery<UserStoryDomain>, UserStoryDomain?>
    {
        public async Task<UserStoryDomain?> Handle(GetByIdQuery<UserStoryDomain> request, CancellationToken cancellationToken)
            => await userStoryRepository.GetByIdAsync(request.Id);
    }
}
