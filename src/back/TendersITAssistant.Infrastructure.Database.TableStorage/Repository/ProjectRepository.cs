using AutoMapper;
using Azure.Data.Tables;
using TendersITAssistant.Domain.Document;
using TendersITAssistant.Domain.Filter;
using TendersITAssistant.Domain.Project;
using TendersITAssistant.Domain.Project.Group;
using TendersITAssistant.Infrastructure.Database.TableStorage.Entity;
using TendersITAssistant.Infrastructure.Database.TableStorage.Extensions;
using TendersITAssistant.Infrastructure.Database.TableStorage.Repository.Generic;

namespace TendersITAssistant.Infrastructure.Database.TableStorage.Repository
{
    internal class ProjectRepository(TableServiceClient service, IMapper mapper, IGenericRepository<DocumentDomain, DocumentEntity> documentRepository, IGenericRepository<UserStoryGroupDomain, UserStoryGroupEntity> groupRepository) : GenericRepository<ProjectDomain, ProjectEntity>(service, "Projects", mapper)
    {
        public async override Task<bool> DeleteAsync(ProjectDomain domain, CancellationToken cancellationToken = default)
        {
            // cascading deletion of all related entities
            var actionResults = await Task.WhenAll([
                ..domain.Documents.Select(item => documentRepository.DeleteAsync(item, cancellationToken)),
                ..domain.Groups.Select(item => groupRepository.DeleteAsync(item, cancellationToken))
                ]);

            if (actionResults.All(x => x)) return false;
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

        public async override Task<bool> UpdateAsync(ProjectDomain domain, CancellationToken cancellationToken = default)
        {
            IEnumerable<DocumentDomain> storedDocuments = await documentRepository.GetAllAsync(new PropertyEqualsFilter(nameof(DocumentEntity.ProjectId), domain.Id), cancellationToken);
            IEnumerable<UserStoryGroupDomain> storedGroups = await groupRepository.GetAllAsync(new PropertyEqualsFilter(nameof(UserStoryGroupEntity.ProjectId), domain.Id), cancellationToken);

            List<Task<bool>> actions = [base.UpdateAsync(domain, cancellationToken)];
            actions.AddRange(domain.Documents.ResolveOperationsWith(
                storedDocuments, documentRepository,
                item => item.ProjectId = domain.Id,
                cancellationToken));
            actions.AddRange(domain.Groups.ResolveOperationsWith(
                storedGroups, groupRepository,
                item => item.ProjectId = domain.Id,
                cancellationToken));

            var actionResults = await Task.WhenAll(actions);
            return actionResults.All(x => x);
        }
    }
}
