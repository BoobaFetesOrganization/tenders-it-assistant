using AutoMapper;
using Azure.Data.Tables;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Domain.Project.Group.UserStory;
using GenAIChat.Infrastructure.Database.TableStorage.Entity;
using GenAIChat.Infrastructure.Database.TableStorage.Extensions;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    internal class UserStoryGroupRepository(TableServiceClient service, IMapper mapper, IGenericRepository<UserStoryRequestDomain, UserStoryRequestEntity> requestRepository, IGenericRepository<UserStoryDomain, UserStoryEntity> storyRepository) : GenericRepository<UserStoryGroupDomain, UserStoryGroupEntity>(service, "UserStoryGroups", mapper)
    {
        public async override Task<UserStoryGroupDomain> AddAsync(UserStoryGroupDomain domain, CancellationToken cancellationToken = default)
        {
            var clone = mapper.Map<UserStoryGroupDomain>(domain);
            clone.Id = TableStorageTools.GetNewId();
            clone.Request.GroupId = clone.Id;

            // create related entity metadata
            clone.Request = await requestRepository.AddAsync(clone.Request, cancellationToken);

            return await base.AddAsync(clone, cancellationToken);
        }

        public async override Task<bool> DeleteAsync(UserStoryGroupDomain domain, CancellationToken cancellationToken = default)
        {
            // cascading deletion of all related entities
            List<Task<bool>> actions = [
                base.DeleteAsync(domain, cancellationToken),
                domain.Request is null ? Task.FromResult(true) : requestRepository.DeleteAsync(domain.Request, cancellationToken),
                .. domain.UserStories.Select(item => storyRepository.DeleteAsync(item, cancellationToken))
                ];
            return (await Task.WhenAll(actions)).All(x => x);

        }

        public async override Task<UserStoryGroupDomain?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await base.GetByIdAsync(id, cancellationToken);
            if (result is null) return null;

            //cascading loading of all related entities 
            var stories = await storyRepository.GetAllAsync(new PropertyEqualsFilter(nameof(UserStoryEntity.GroupId), id), cancellationToken);

            result.Request = (await requestRepository.GetAllAsync(new PropertyEqualsFilter(nameof(UserStoryRequestEntity.GroupId), result.Id), cancellationToken)).Single();
            result.UserStories = [.. await Task.WhenAll(stories.Select(i => storyRepository.GetByIdAsync(i.Id, cancellationToken)))];

            return result;
        }

        public async override Task<bool> UpdateAsync(UserStoryGroupDomain domain, CancellationToken cancellationToken = default)
        {
            var storedStories = await storyRepository.GetAllAsync(new PropertyEqualsFilter(nameof(UserStoryEntity.GroupId), domain.Id), cancellationToken);

            List<Task<bool>> actions = [
                base.UpdateAsync(domain, cancellationToken),
                requestRepository.UpdateAsync(domain.Request, cancellationToken)
                ];
            actions.AddRange(domain.UserStories.ResolveOperationsWith(
                storedStories, storyRepository,
                item => item.GroupId = domain.Id,
                cancellationToken));

            var actionResults = await Task.WhenAll(actions);
            return actionResults.All(x => x);
        }
    }
}
