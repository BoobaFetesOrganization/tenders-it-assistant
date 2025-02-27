using GenAIChat.Domain.Project.Group;

namespace GenAIChat.Application.Usecase.Interface
{
    internal interface IUserStoryGroupApplication : IApplication<UserStoryGroupDomain>
    {
        public Task<UserStoryGroupDomain> GenerateUserStoriesAsync(string projectId, string groupId, CancellationToken cancellationToken);

        public Task<UserStoryGroupDomain> ValidateCostsAsync(string projectId, string groupId, CancellationToken cancellationToken);

    }
}
