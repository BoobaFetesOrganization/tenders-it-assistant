using GenAIChat.Domain.Common;

namespace GenAIChat.Domain.Project.Group.UserStory.Task.Cost
{
    public class TaskCostDomain : IEntityDomain
    {
        public int Id { get; set; }

        public TaskCostKind Kind { get; set; } = TaskCostKind.Gemini;
        public double Cost { get; set; } = 0;

        #region  navigation properties
        public int TaskId { get; set; }
        #endregion

        public TaskCostDomain() { }
        public TaskCostDomain(double cost, TaskCostKind kind)
        {
            Cost = cost;
            Kind = kind;
        }
        public TaskCostDomain(TaskCostDomain entity)
        {
            Id = entity.Id;
            TaskId = entity.TaskId;
            Kind = entity.Kind;
            Cost = entity.Cost;
        }
    }
}