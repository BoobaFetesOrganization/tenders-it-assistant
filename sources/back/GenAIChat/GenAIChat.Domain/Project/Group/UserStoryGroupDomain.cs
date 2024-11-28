using GenAIChat.Domain.Common;
using GenAIChat.Domain.Project.Group.UserStory;

namespace GenAIChat.Domain.Project.Group
{

    public class UserStoryGroupDomain : IEntityDomain
    {
        public int Id { get; set; }

        public UserStoryPromptDomain Prompt { get; set; } = new UserStoryPromptDomain();
        public string? PromptResponse { get; set; } = null;

        public ICollection<UserStoryDomain> UserStories { get; set; } = [];

        #region  navigation properties
        public int ProjectId { get; set; }
        #endregion

        public UserStoryGroupDomain() { }

        public UserStoryGroupDomain(UserStoryPromptDomain prompt)
        {
            Prompt = new UserStoryPromptDomain(prompt);
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