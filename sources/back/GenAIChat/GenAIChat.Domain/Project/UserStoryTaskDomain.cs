using GenAIChat.Domain.Common;

namespace GenAIChat.Domain.Project
{
    public class UserStoryTaskDomain : IEntityDomain
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Cost { get; set; } = 0;

        #region  navigation properties
        public int UserStoryId { get; set; }
        #endregion

    }
}