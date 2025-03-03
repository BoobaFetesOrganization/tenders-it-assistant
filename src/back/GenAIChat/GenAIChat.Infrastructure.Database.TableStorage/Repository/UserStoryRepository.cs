using AutoMapper;
using Azure.Data.Tables;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project.Group.UserStory;
using GenAIChat.Domain.Project.Group.UserStory.Task;
using GenAIChat.Infrastructure.Database.TableStorage.Entity;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    internal class UserStoryRepository(TableServiceClient service, IMapper mapper, IRepositoryAdapter<TaskDomain> taskRepository) : GenericRepository<UserStoryDomain, UserStoryEntity>(service, "UserStorys", mapper)
    {
        public async virtual Task<UserStoryDomain> AddAsync(UserStoryDomain domain, CancellationToken cancellationToken = default)
        {
            var clone = mapper.Map<UserStoryDomain>(domain);
            clone.Id = Tools.GetNewId();

            foreach (var task in clone.Tasks) task.UserStoryId = clone.Id;

            clone.Tasks = [.. await Task.WhenAll(domain.Tasks.Select(item => taskRepository.AddAsync(item, cancellationToken)))];
            
            return await base.AddAsync(clone, cancellationToken);
        }

        public async override Task<bool?> DeleteAsync(UserStoryDomain domain, CancellationToken cancellationToken = default)
        {
            // cascading deletion of all related entities
            var actionResults = await Task.WhenAll([.. domain.Tasks.Select(item => taskRepository.DeleteAsync(item, cancellationToken))]);

            if (!actionResults.All(x => x.HasValue && x.Value)) return false;
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
    }
}
