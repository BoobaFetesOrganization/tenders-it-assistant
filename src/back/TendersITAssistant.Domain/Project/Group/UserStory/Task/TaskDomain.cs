using TendersITAssistant.Domain.Common;
using TendersITAssistant.Domain.Project.Group.UserStory.Task.Cost;

namespace TendersITAssistant.Domain.Project.Group.UserStory.Task
{
    public class TaskDomain : EntityDomain
    {
        public string Name { get; set; } = string.Empty;
        public double Cost { get; set; } = 0;
        public ICollection<TaskCostDomain> WorkingCosts { get; set; } = [];

        #region  navigation properties
        public string UserStoryId { get; set; } = string.Empty;
        #endregion

        public void AddGeminiCost(double cost)
        {
            WorkingCosts.Add(new() { Cost = cost, Kind = TaskCostKind.Gemini });
        }
    }
}