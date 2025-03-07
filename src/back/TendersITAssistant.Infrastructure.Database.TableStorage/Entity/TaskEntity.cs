using TendersITAssistant.Infrastructure.Database.TableStorage.Entity.Common;

namespace TendersITAssistant.Infrastructure.Database.TableStorage.Entity
{
    internal class TaskEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public double Cost { get; set; } = 0;

        #region  navigation properties
        public string UserStoryId { get; set; } = string.Empty;
        #endregion
    }
}