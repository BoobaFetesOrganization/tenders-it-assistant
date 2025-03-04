using AutoMapper;
using Azure.Data.Tables;
using GenAIChat.Domain.Project.Group.UserStory.Task.Cost;
using GenAIChat.Infrastructure.Database.TableStorage.Entity;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    internal class TaskCostRepository(TableServiceClient service, IMapper mapper) : GenericRepository<TaskCostDomain, TaskCostEntity>(service, "TaskCosts", mapper)
    {
    }
}
