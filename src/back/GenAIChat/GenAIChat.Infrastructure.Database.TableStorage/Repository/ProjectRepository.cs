using AutoMapper;
using Azure.Data.Tables;
using GenAIChat.Application.Specifications;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project;
using GenAIChat.Infrastructure.Database.TableStorage.Entity;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Common;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class ProjectRepository(TableServiceClient service, IMapper mapper) : BaseRepository<ProjectDomain>(service, "Projects")
    {
        public async override Task<ProjectDomain> AddAsync(ProjectDomain domain)
        {
            var entity = mapper.Map<ProjectEntity>(domain);
            await client.AddEntityAsync(entity);
            var result = await GetByIdAsync(entity);
            return result!;
        }

        public override Task<int> CountAsync(IFilter? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<ProjectDomain> DeleteAsync(ProjectDomain domain)
        {
            throw new NotImplementedException();
        }

        public async override Task<IEnumerable<ProjectDomain>> GetAllAsync(IFilter? filter = null)
        {
            var filterString = filter is null ? null : filter.ToAzureFilterString();
            var results = client.Query<ProjectEntity>(filterString).ToArray();
            return await Task.FromResult(mapper.Map<IEnumerable<ProjectDomain>>(results));
        }

        public override Task<Paged<ProjectDomain>> GetAllPagedAsync(PaginationOptions options, IFilter? filter = null)
        {
            throw new NotImplementedException();
        }

        public async override Task<ProjectDomain?> GetByIdAsync(string id) => await GetByIdAsync(new ProjectEntity(id));

        private async Task<ProjectDomain?> GetByIdAsync(ProjectEntity entity)
        {
            var filter = new AndFilter(
                new PropertyEqualsFilter(nameof(ITableEntity.PartitionKey), entity.PartitionKey),
                new PropertyEqualsFilter(nameof(ITableEntity.RowKey), entity.RowKey)
                );

            var results = client.Query<ProjectEntity>(filter.ToAzureFilterString()).FirstOrDefault();

            return await Task.FromResult(results is null ? null : mapper.Map<ProjectDomain>(results));
        }

        public override Task<ProjectDomain> UpdateAsync(ProjectDomain domain)
        {
            throw new NotImplementedException();
        }
    }
}
