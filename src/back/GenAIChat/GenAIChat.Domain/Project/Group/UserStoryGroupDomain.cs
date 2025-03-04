using GenAIChat.Domain.Common;
using GenAIChat.Domain.Project.Group.UserStory;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenAIChat.Domain.Project.Group
{
    public class UserStoryGroupDomain : EntityDomain
    {
        public UserStoryRequestDomain Request { get; set; } = new UserStoryRequestDomain();
        public string? Response { get; set; } = null;

        [NotMapped]
        public double Cost { get => UserStories.Sum(us => us.Cost); }

        public ICollection<UserStoryDomain> UserStories { get; set; } = [];

        #region  navigation properties
        public string ProjectId { get; set; } = string.Empty;
        #endregion

        public void ClearUserStories() => UserStories.Clear();

        public void AddManyStory(IEnumerable<UserStoryDomain>? stories)
        {
            if (stories is null) return;
            foreach (var story in stories) AddStory(story);
        }
        public void AddStory(UserStoryDomain userStory)
        {
            userStory.GroupId = Id;
            UserStories.Add(userStory);
        }
        public bool RemoveStory(UserStoryDomain userStory)
        {
            return UserStories.Remove(userStory);
        }
    }
}