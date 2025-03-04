using AutoMapper;
using Azure.Data.Tables;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Infrastructure.Database.TableStorage.Entity;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    internal class ProjectRepository(TableServiceClient service, IMapper mapper, IRepositoryAdapter<DocumentDomain> documentRepository, IRepositoryAdapter<UserStoryGroupDomain> groupRepository) : GenericRepository<ProjectDomain, ProjectEntity>(service, "Projects", mapper)
    {
        public async override Task<bool?> DeleteAsync(ProjectDomain domain, CancellationToken cancellationToken = default)
        {
            // cascading deletion of all related entities
            var actionResults = await Task.WhenAll([
                ..domain.Documents.Select(item => documentRepository.DeleteAsync(item, cancellationToken)),
                ..domain.Groups.Select(item => groupRepository.DeleteAsync(item, cancellationToken))
                ]);

            if (!actionResults.All(x => x.HasValue && x.Value)) return false;
            return await base.DeleteAsync(domain, cancellationToken);
        }

        public async override Task<ProjectDomain?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await base.GetByIdAsync(id, cancellationToken);
            if (result is null) return null;

            //cascading loading of all related entities 
            var documents = await documentRepository.GetAllAsync(new PropertyEqualsFilter(nameof(DocumentEntity.ProjectId), id), cancellationToken);
            var groups = await groupRepository.GetAllAsync(new PropertyEqualsFilter(nameof(UserStoryGroupEntity.ProjectId), id), cancellationToken);

            result.Documents = [.. await Task.WhenAll(documents.Select(i => documentRepository.GetByIdAsync(i.Id, cancellationToken)))];
            result.Groups = [.. await Task.WhenAll(groups.Select(i => groupRepository.GetByIdAsync(i.Id, cancellationToken)))];

            return result;
        }
    }
}
