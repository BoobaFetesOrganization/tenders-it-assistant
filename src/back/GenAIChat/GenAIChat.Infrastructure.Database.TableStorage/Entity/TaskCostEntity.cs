using GenAIChat.Domain.Project.Group.UserStory.Task.Cost;
using GenAIChat.Infrastructure.Database.TableStorage.Entity.Common;

namespace GenAIChat.Infrastructure.Database.TableStorage.Entity
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