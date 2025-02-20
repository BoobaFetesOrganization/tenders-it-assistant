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

        public override object Clone()
        {
            UserStoryGroupDomain clone = new()
            {
                Request = (UserStoryPromptDomain)Request.Clone(),
                Response = Response,
                ProjectId = ProjectId
            };

            foreach (var item in UserStories) clone.UserStories.Add((UserStoryDomain)item.Clone());

            return clone;
        }
    }
}