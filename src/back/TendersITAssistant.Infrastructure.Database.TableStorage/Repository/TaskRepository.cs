using AutoMapper;
using Azure.Data.Tables;
using TendersITAssistant.Domain.Filter;
using TendersITAssistant.Domain.Project.Group.UserStory.Task;
using TendersITAssistant.Domain.Project.Group.UserStory.Task.Cost;
using TendersITAssistant.Infrastructure.Database.TableStorage.Entity;
using TendersITAssistant.Infrastructure.Database.TableStorage.Extensions;
using TendersITAssistant.Infrastructure.Database.TableStorage.Repository.Generic;

namespace TendersITAssistant.Infrastructure.Database.TableStorage.Repository
{
    internal class TaskRepository(TableServiceClient service, IMapper mapper, IGenericRepository<TaskCostDomain, TaskCostEntity> taskcostRepository) : GenericRepository<TaskDomain, TaskEntity>(service, "Tasks", mapper)
    {
        public async override Task<TaskDomain> AddAsync(TaskDomain domain, CancellationToken cancellationToken = default)
        {
            var clone = mapper.Map<TaskDomain>(domain);
            clone.Id = TableStorageTools.GetNewId();

            clone.WorkingCosts = [.. await Task.WhenAll(clone.WorkingCosts.Select(item => {
                item.TaskId = clone.Id;
                return taskcostRepository.AddAsync(item, cancellationToken);
            }))];

            return await base.AddAsync(clone, cancellationToken);
        }
        public async override Task<bool> DeleteAsync(TaskDomain domain, CancellationToken cancellationToken = default)
        {
            // cascading deletion of all related entities
            var actionResults = await Task.WhenAll([.. domain.WorkingCosts.Select(item => taskcostRepository.DeleteAsync(item, cancellationToken))]);

            if (!actionResults.All(x => x)) return false;
            return await base.DeleteAsync(domain, cancellationToken);
        }

        public async override Task<TaskDomain?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await base.GetByIdAsync(id, cancellationToken);
            if (result is null) return null;

            //cascading loading of all related entities 
            var costs = await taskcostRepository.GetAllAsync(new PropertyEqualsFilter(nameof(TaskCostEntity.TaskId), id), cancellationToken);

            result.WorkingCosts = [.. await Task.WhenAll(costs.Select(i => taskcostRepository.GetByIdAsync(i.Id, cancellationToken)))];

            return result;
        }

        public async override Task<bool> UpdateAsync(TaskDomain domain, CancellationToken cancellationToken = default)
        {
            var storedCosts = await taskcostRepository.GetAllAsync(new PropertyEqualsFilter(nameof(TaskCostEntity.TaskId), domain.Id), cancellationToken);

            List<Task<bool>> actions = [base.UpdateAsync(domain, cancellationToken)];
            actions.AddRange(domain.WorkingCosts.ResolveOperationsWith(storedCosts, taskcostRepository,
                item => item.TaskId = domain.Id,
                cancellationToken));

            var actionResults = await Task.WhenAll(actions);
            return actionResults.All(x => x);
        }
    }
}
