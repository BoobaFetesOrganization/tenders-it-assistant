using GenAIChat.Domain.Common;
using GenAIChat.Domain.Project.Group.UserStory;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenAIChat.Domain.Project.Group
{

    public class UserStoryGroupDomain : EntityDomain
    {
        public UserStoryPromptDomain Request { get; set; } = new UserStoryPromptDomain();
        public string? Response { get; set; } = null;

        [NotMapped]
        public double Cost { get => UserStories.Sum(us => us.Cost); }

        public ICollection<UserStoryDomain> UserStories { get; set; } = [];

        #region  navigation properties
        public string ProjectId { get; set; } = string.Empty;
        #endregion

        public UserStoryGroupDomain() { }

        public UserStoryGroupDomain(UserStoryGroupDomain domain) : base(domain)
        {
            Request = new UserStoryPromptDomain(domain.Request);
            Response = domain.Response;
            UserStories = [.. domain.UserStories];
            ProjectId = domain.ProjectId;
        }

        public UserStoryGroupDomain(UserStoryPromptDomain prompt)
        {
            Request = new UserStoryPromptDomain(prompt);
        }

        public void ClearUserStories() => UserStories.Clear();

        public void SetUserStory(IEnumerable<UserStoryDomain>? stories)
        {
            UserStories.Clear();
            if (stories is null) return;
            foreach (var story in stories) AddUserStory(story);
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