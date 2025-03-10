using AutoMapper;
using Azure.Data.Tables;
using TendersITAssistant.Domain.Filter;
using TendersITAssistant.Domain.Project.Group.UserStory;
using TendersITAssistant.Domain.Project.Group.UserStory.Task;
using TendersITAssistant.Infrastructure.Database.TableStorage.Entity;
using TendersITAssistant.Infrastructure.Database.TableStorage.Extensions;
using TendersITAssistant.Infrastructure.Database.TableStorage.Repository.Generic;

namespace TendersITAssistant.Infrastructure.Database.TableStorage.Repository
{
    internal class UserStoryRepository(TableServiceClient service, IMapper mapper, IGenericRepository<TaskDomain, TaskEntity> taskRepository) : GenericRepository<UserStoryDomain, UserStoryEntity>(service, "UserStorys", mapper)
    {
        public async override Task<UserStoryDomain> AddAsync(UserStoryDomain domain, CancellationToken cancellationToken = default)
        {
            var clone = mapper.Map<UserStoryDomain>(domain);
            clone.Id = TableStorageTools.GetNewId();

            clone.Tasks = await Task.WhenAll(domain.Tasks.Select(item =>
            {
                item.UserStoryId = clone.Id;
                return taskRepository.AddAsync(item, cancellationToken);
            }));

            return await base.AddAsync(clone, cancellationToken);
        }

        public async override Task<bool> DeleteAsync(UserStoryDomain domain, CancellationToken cancellationToken = default)
        {
            // cascading deletion of all related entities
            var actionResults = await Task.WhenAll(domain.Tasks.Select(item => taskRepository.DeleteAsync(item, cancellationToken)));

            if (!actionResults.All(x => x)) return false;
            return await base.DeleteAsync(domain, cancellationToken);
        }

        public async override Task<UserStoryDomain?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await base.GetByIdAsync(id, cancellationToken);
            if (result is null) return null;

            //cascading loading of all related entities 
            var tasks = await taskRepository.GetAllAsync(new PropertyEqualsFilter(nameof(TaskDomain.UserStoryId), id), cancellationToken);

            result.Tasks = [.. await Task.WhenAll(tasks.Select(i => taskRepository.GetByIdAsync(i.Id, cancellationToken)))];

            return result;
        }

        public async override Task<bool> UpdateAsync(UserStoryDomain domain, CancellationToken cancellationToken = default)
        {
            var storedTasks = await taskRepository.GetAllAsync(new PropertyEqualsFilter(nameof(TaskEntity.UserStoryId), domain.Id), cancellationToken);

            List<Task<bool>> actions = [base.UpdateAsync(domain, cancellationToken)];
            actions.AddRange(domain.Tasks.ResolveOperationsWith(
                storedTasks, taskRepository,
                item => item.UserStoryId = domain.Id,
                cancellationToken));

            var actionResults = await Task.WhenAll(actions);
            return actionResults.All(x => x);
        }
    }
}
