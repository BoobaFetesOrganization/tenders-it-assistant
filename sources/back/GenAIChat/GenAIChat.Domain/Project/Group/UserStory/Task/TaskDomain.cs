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

        public void AddGeminiCost(double cost)
        {
            WorkingCosts.Add(new TaskCostDomain(cost, TaskCostKind.Gemini));
        }
    }
}