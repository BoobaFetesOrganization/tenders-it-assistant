using GenAIChat.Domain.Common;
using GenAIChat.Domain.Project.Group.UserStory;

namespace GenAIChat.Domain.Project.Group
{

    public class UserStoryGroupDomain : IEntityDomain
    {
        public int Id { get; set; }

        public UserStoryPromptDomain Request { get; set; } = new UserStoryPromptDomain();
        public string? Response { get; set; } = null;

        public ICollection<UserStoryDomain> UserStories { get; set; } = [];

        #region  navigation properties
        public int ProjectId { get; set; }
        #endregion

        public UserStoryGroupDomain() { }

        public UserStoryGroupDomain(UserStoryGroupDomain group)
        {
            Id = group.Id;
            Request = new UserStoryPromptDomain(group.Request);
            Response = group.Response;
            UserStories = [.. group.UserStories];
            ProjectId = group.ProjectId;
        }

        public UserStoryGroupDomain(UserStoryPromptDomain prompt)
        {
            Request = new UserStoryPromptDomain(prompt);
        }

        public void AddUserStory(UserStoryDomain userStory)
        {
            userStory.GroupId = Id;
            UserStories.Add(userStory);
        }
        public bool RemoveUserStory(UserStoryDomain userStory)
        {
            return UserStories.Remove(userStory);
        }
    }
}