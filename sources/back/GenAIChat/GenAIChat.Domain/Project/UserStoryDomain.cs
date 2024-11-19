using GenAIChat.Domain.Common;

namespace GenAIChat.Domain.Project
{
    public class UserStoryDomain : IEntityDomain
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<UserStoryTaskDomain> Tasks { get; set; } = [];
        public double Cost { get => Tasks.Sum(t => t.Cost); }

        #region  navigation properties
        public int ProjectId { get; set; }
        #endregion

    }
}