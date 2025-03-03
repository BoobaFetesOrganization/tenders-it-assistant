using AutoMapper;
using Azure;
using Azure.Data.Tables;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Domain.Project.Group.UserStory;
using GenAIChat.Infrastructure.Database.TableStorage.Entity;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Generic;
using System.Net;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    internal class UserStoryGroupRepository(TableServiceClient service, IMapper mapper, IRepositoryAdapter<UserStoryRequestDomain> requestRepository, IRepositoryAdapter<UserStoryDomain> storyRepository) : GenericRepository<UserStoryGroupDomain, UserStoryGroupEntity>(service, "UserStoryGroups", mapper)
    {
        public async override Task<UserStoryGroupDomain> AddAsync(UserStoryGroupDomain domain, CancellationToken cancellationToken = default)
        {
            var clone = mapper.Map<UserStoryGroupDomain>(domain);
            clone.Id = Tools.GetNewId();
            clone.Request.GroupId = clone.Id;

            // create related entity metadata
            clone.Request = await requestRepository.AddAsync(clone.Request, cancellationToken);

            return await base.AddAsync(clone, cancellationToken);
        }

        public async override Task<bool?> DeleteAsync(UserStoryGroupDomain domain, CancellationToken cancellationToken = default)
        {
            // cascading deletion of all related entities
            List<Task<bool?>> actions = [.. domain.UserStories.Select(item => storyRepository.DeleteAsync(item, cancellationToken))];
            if (domain.Request != null) actions.AddRange(requestRepository.DeleteAsync(domain.Request, cancellationToken));
            var actionResults = await Task.WhenAll(actions);

            if (!actionResults.All(x => x.HasValue && x.Value)) return false;
            return await base.DeleteAsync(domain, cancellationToken);
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
    }
}
