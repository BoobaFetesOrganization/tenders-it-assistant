using Azure.Data.Tables;
using GenAIChat.Domain.Project.Group.UserStory.Task;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class TaskRepository(TableServiceClient service) : GenericRepository<TaskDomain>(service, "Tasks")
    {
    }
}
