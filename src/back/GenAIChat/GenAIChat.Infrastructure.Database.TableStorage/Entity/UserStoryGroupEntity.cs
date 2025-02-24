using GenAIChat.Infrastructure.Database.TableStorage.Entity.Common;

namespace GenAIChat.Infrastructure.Database.TableStorage.Entity
{
    internal class UserStoryGroupEntity : BaseEntity
    {

        public string RequestId { get; set; } = string.Empty;
        public string? Response { get; set; } = null;

        #region  navigation properties
        public string ProjectId { get; set; } = string.Empty;
        #endregion
    }
}