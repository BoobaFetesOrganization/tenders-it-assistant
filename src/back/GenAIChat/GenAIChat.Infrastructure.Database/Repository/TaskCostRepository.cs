using GenAIChat.Application.Adapter.Database.Repository;
using GenAIChat.Domain.Project.Group.UserStory.Task.Cost;
using GenAIChat.Infrastructure.Database.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.Repository
{
    public class TaskCostRepository(GenAiDbContext dbContext) : GenericRepository<TaskCostDomain>(dbContext), ITaskCostRepositoryAdapter
    {
    }
}
