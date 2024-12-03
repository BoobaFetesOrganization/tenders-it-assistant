using GenAIChat.Domain.Common;
using GenAIChat.Domain.Project.Group.UserStory.Task.Cost;

namespace GenAIChat.Domain.Project.Group.UserStory.Task
{
    public class TaskDomain : IEntityDomain
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Cost { get; set; } = 0;
        public ICollection<TaskCostDomain> WorkingCosts { get; set; } = [];

        #region  navigation properties
        public int UserStoryId { get; set; }
        #endregion

        public TaskDomain() { }
        public TaskDomain(string name, double cost)
        {
            Name = name;
            Cost = cost;
        }

        public TaskDomain(TaskDomain entity)
        {
            Id = entity.Id;
            UserStoryId = entity.UserStoryId;
            Name = entity.Name;
            Cost = entity.Cost;
            WorkingCosts = entity.WorkingCosts;
        }

        public void AddCost(TaskCostDomain cost)
        {
            cost.TaskId = Id;
            WorkingCosts.Add(cost);
        }
        public bool RemoveCost(TaskCostDomain cost)
        {
            return WorkingCosts.Remove(cost);
        }
    }
}