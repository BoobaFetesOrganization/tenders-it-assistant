using GenAIChat.Domain.Project.Group.UserStory.Task.Cost;
using GenAIChat.Infrastructure.Database.Sqlite.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.Sqlite.Repository
{
    public class TaskCostRepository(GenAiDbContext dbContext) : GenericRepository<TaskCostDomain>(dbContext)
    {
    }
}
