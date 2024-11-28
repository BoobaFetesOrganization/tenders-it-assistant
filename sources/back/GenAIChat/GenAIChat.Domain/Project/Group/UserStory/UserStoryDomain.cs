using GenAIChat.Domain.Common;
using GenAIChat.Domain.Project.Group.UserStory.Task;

namespace GenAIChat.Domain.Project.Group.UserStory
{
    public class UserStoryDomain : IEntityDomain
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<TaskDomain> Tasks { get; set; } = [];
        public double Cost { get => Tasks.Sum(t => t.Cost); }

        #region  navigation properties
        public int GroupId { get; set; }
        #endregion

        public UserStoryDomain() { }

        public UserStoryDomain(string name, int groupId, ICollection<TaskDomain>? tasks = null, int id = 0)
        {
            Id = id;
            Name = name;
            GroupId = groupId;
            Tasks = tasks ?? [];
        }
        public UserStoryDomain(UserStoryDomain entity, bool creation = false)
        {
            if (!creation) Id = entity.Id;
            Name = entity.Name;
            GroupId = entity.GroupId;
            Tasks = entity.Tasks;
        }

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