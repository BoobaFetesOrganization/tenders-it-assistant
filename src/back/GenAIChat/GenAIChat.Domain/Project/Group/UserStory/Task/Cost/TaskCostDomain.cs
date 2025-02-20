using GenAIChat.Domain.Common;

namespace GenAIChat.Domain.Project.Group.UserStory.Task.Cost
{
    public class TaskCostDomain : EntityDomain
    {
        public TaskCostKind Kind { get; set; } = TaskCostKind.Gemini;
        public double Cost { get; set; } = 0;

        #region  navigation properties
        public string TaskId { get; set; } = string.Empty;
        #endregion

        public override object Clone()
        {
            TaskCostDomain clone = new();

            clone.Kind = Kind;
            clone.Cost = Cost;
            clone.TaskId = TaskId;

            return clone;
        }
    }
}