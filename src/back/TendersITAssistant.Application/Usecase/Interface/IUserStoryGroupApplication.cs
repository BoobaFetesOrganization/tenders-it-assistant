using TendersITAssistant.Domain.Project;
using TendersITAssistant.Domain.Project.Group;

namespace TendersITAssistant.Application.Usecase.Interface
{
    public interface IUserStoryGroupApplication : IApplication<UserStoryGroupDomain>
    {
        public Task<UserStoryGroupDomain> CreateAsync(string projectId, CancellationToken cancellationToken = default);

        public Task<UserStoryGroupDomain> GenerateUserStoriesAsync(string projectId, string groupId, CancellationToken cancellationToken = default);
        public Task<UserStoryGroupDomain> GenerateUserStoriesAsync(ProjectDomain? project, UserStoryGroupDomain? group, CancellationToken cancellationToken = default);

        public Task<UserStoryGroupDomain> ValidateCostsAsync(string projectId, string groupId, CancellationToken cancellationToken = default);
    }
}
