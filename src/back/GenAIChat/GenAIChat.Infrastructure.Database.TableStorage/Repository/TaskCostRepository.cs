using Azure.Data.Tables;
using GenAIChat.Domain.Project.Group.UserStory.Task.Cost;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class TaskCostRepository(TableServiceClient service) : GenericRepository<TaskCostDomain>(service, "TaskCosts")
    {
    }
}
