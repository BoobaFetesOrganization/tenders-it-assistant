using TendersITAssistant.Domain.Common;

namespace TendersITAssistant.Domain.Project.Group.UserStory.Task.Cost
{
    public class TaskCostDomain : EntityDomain
    {
        public TaskCostKind Kind { get; set; } = TaskCostKind.Gemini;
        public double Cost { get; set; } = 0;

        #region  navigation properties
        public string TaskId { get; set; } = string.Empty;
        #endregion
    }
}