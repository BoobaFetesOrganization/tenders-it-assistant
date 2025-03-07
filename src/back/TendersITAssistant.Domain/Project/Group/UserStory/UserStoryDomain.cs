using TendersITAssistant.Domain.Common;
using TendersITAssistant.Domain.Project.Group.UserStory.Task;

namespace TendersITAssistant.Domain.Project.Group.UserStory
{
    public class UserStoryDomain : EntityDomain
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<TaskDomain> Tasks { get; set; } = [];
        public double Cost { get => Tasks.Sum(t => t.Cost); }

        #region  navigation properties
        public string GroupId { get; set; } = string.Empty;
        #endregion

        public void AddTask(TaskDomain task)
        {
            task.UserStoryId = Id;
            Tasks.Add(task);
        }
        public bool RemoveTask(TaskDomain task)
        {
            return Tasks.Remove(task);
        }
    }
}