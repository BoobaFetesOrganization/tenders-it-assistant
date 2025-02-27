using GenAIChat.Infrastructure.Database.TableStorage.Entity.Common;

namespace GenAIChat.Infrastructure.Database.TableStorage.Entity
{
    internal class UserStoryRequestEntity : BaseEntity
    {
        public string Context { get; set; } = string.Empty;
        public string Personas { get; set; } = string.Empty;
        public string Tasks { get; set; } = string.Empty;

        #region  navigation properties
        public string GroupId { get; set; } = string.Empty;
        #endregion
    }
}
