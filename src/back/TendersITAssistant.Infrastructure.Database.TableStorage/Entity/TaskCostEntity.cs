using TendersITAssistant.Domain.Project.Group.UserStory.Task.Cost;
using TendersITAssistant.Infrastructure.Database.TableStorage.Entity.Common;

namespace TendersITAssistant.Infrastructure.Database.TableStorage.Entity
{
    internal class TaskCostEntity : BaseEntity
    {
        public TaskCostKind Kind { get; set; }
        public double Cost { get; set; }

        #region  navigation properties
        public string TaskId { get; set; } = string.Empty;
        #endregion
    }
}