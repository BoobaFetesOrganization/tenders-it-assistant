using AutoMapper;
using Azure.Data.Tables;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project.Group.UserStory.Task;
using GenAIChat.Domain.Project.Group.UserStory.Task.Cost;
using GenAIChat.Infrastructure.Database.TableStorage.Entity;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    internal class TaskRepository(TableServiceClient service, IMapper mapper, IRepositoryAdapter<TaskCostDomain> taskcostRepository) : GenericRepository<TaskDomain, TaskEntity>(service, "Tasks", mapper)
    {
        public async override Task<TaskDomain> AddAsync(TaskDomain domain, CancellationToken cancellationToken = default)
        {
            var clone = mapper.Map<TaskDomain>(domain);
            clone.Id = Tools.GetNewId();

            foreach (var cost in clone.WorkingCosts) cost.TaskId = clone.Id;

            clone.WorkingCosts = [.. await Task.WhenAll(clone.WorkingCosts.Select(item => taskcostRepository.AddAsync(item, cancellationToken)))];

            return await base.AddAsync(clone, cancellationToken);
        }
        public async override Task<bool?> DeleteAsync(TaskDomain domain, CancellationToken cancellationToken = default)
        {
            // cascading deletion of all related entities
            var actionResults = await Task.WhenAll([.. domain.WorkingCosts.Select(item => taskcostRepository.DeleteAsync(item, cancellationToken))]);

            if (!actionResults.All(x => x.HasValue && x.Value)) return false;
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

        public async override Task<bool?> UpdateAsync(TaskDomain domain, CancellationToken cancellationToken = default)
        {
            var clone = mapper.Map<TaskDomain>(domain);
            clone.Id = Tools.GetNewId();

            foreach (var cost in clone.WorkingCosts) cost.TaskId = clone.Id;

            await Task.WhenAll(clone.WorkingCosts.Select(item => taskcostRepository.UpdateAsync(item, cancellationToken)));

            return await base.UpdateAsync(clone, cancellationToken);
        }
    }
}
