using GenAIChat.Application.Command.Project.Group;
using GenAIChat.Application.Resources;
using GenAIChat.Application.Usecase.Common;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Domain.Project.Group.UserStory;
using MediatR;

namespace GenAIChat.Application.Usecase
{
    public class UserStoryGroupApplication(IMediator mediator, EmbeddedResource resource) : ApplicationBase<UserStoryGroupDomain>(mediator)
    {
        public override Task<UserStoryGroupDomain> CreateAsync(UserStoryGroupDomain domain)
        {
            throw new Exception("Use CreateAsync(int projectId) instead, because a group needs to be initialized from the server only");
        }

        public async Task<UserStoryGroupDomain?> UpdateUserStoriesAsync(string id, ICollection<UserStoryDomain> userStories)
        {
            UserStoryGroupDomain group = await GetByIdAsync(id) ?? throw new Exception("UserStoryGroup not found");

            group.UserStories = userStories;

            return await base.UpdateAsync(group);
        }
    }
}
