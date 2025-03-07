using AutoMapper;
using Azure.Data.Tables;
using TendersITAssistant.Domain.Project.Group.UserStory.Task.Cost;
using TendersITAssistant.Infrastructure.Database.TableStorage.Entity;
using TendersITAssistant.Infrastructure.Database.TableStorage.Repository.Generic;

namespace TendersITAssistant.Infrastructure.Database.TableStorage.Repository
{
    internal class TaskCostRepository(TableServiceClient service, IMapper mapper) : GenericRepository<TaskCostDomain, TaskCostEntity>(service, "TaskCosts", mapper)
    {
    }
}
