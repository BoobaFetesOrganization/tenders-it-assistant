using TendersITAssistant.Infrastructure.Database.TableStorage.Entity.Common;

namespace TendersITAssistant.Infrastructure.Database.TableStorage.Entity
{
    internal class UserStoryEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        #region  navigation properties
        public string GroupId { get; set; } = string.Empty;
        #endregion
    }
}