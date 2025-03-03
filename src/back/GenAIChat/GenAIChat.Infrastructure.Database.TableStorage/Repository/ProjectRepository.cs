using AutoMapper;
using Azure;
using Azure.Data.Tables;
using GenAIChat.Application.Specifications;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project;
using GenAIChat.Infrastructure.Database.TableStorage.Entity;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Common;
using System.Net;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class ProjectRepository(TableServiceClient service, IMapper mapper) : BaseRepository<ProjectDomain>(service, "Projects")
    {
        public async override Task<ProjectDomain> AddAsync(ProjectDomain domain, CancellationToken cancellationToken = default)
        {
            var entity = mapper.Map<ProjectEntity>(domain);
            await client.AddEntityAsync(entity, cancellationToken);
            var result = await GetByIdAsync(entity, cancellationToken);
            return result!;
        }

        public override async Task<int> CountAsync(IFilter? filter = null, CancellationToken cancellationToken = default)
        {
            var filterString = filter?.ToAzureFilterString();
            var count = client.Query<ProjectEntity>(filterString, null, null, cancellationToken).Count();
            return await Task.FromResult(count);
        }

        public override async Task<bool?> DeleteAsync(ProjectDomain domain, CancellationToken cancellationToken = default)
        {
            var entity = mapper.Map<ProjectEntity>(domain);
            try
            {
                await client.DeleteEntityAsync(entity.PartitionKey, entity.RowKey, default, cancellationToken);
                return true;
            }
            catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.NoContent)
            {
                return false;
            }
            catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.NotFound)
            {
                return null;
            }
            catch (Exception) // on sait jamais, faudra revoir ca un jour ....
            {
                return null;
            }
        }

        public async override Task<IEnumerable<ProjectDomain>> GetAllAsync(IFilter? filter = null, CancellationToken cancellationToken = default)
        {
            var results = client.Query<ProjectEntity>(filter?.ToAzureFilterString(), null, null, cancellationToken).ToArray();
            return await Task.FromResult(mapper.Map<IEnumerable<ProjectDomain>>(results));
        }

        public async override Task<Paged<ProjectDomain>> GetAllPagedAsync(PaginationOptions options, IFilter? filter = null, CancellationToken cancellationToken = default)
        {
            var filterString = filter?.ToAzureFilterString();

            var count = await CountAsync(filter, cancellationToken);
            var data = client.Query<ProjectEntity>(filterString, null, null, cancellationToken)
                                .Skip(options.Offset)
                                .Take(options.Limit)
                                .ToArray();

            return new Paged<ProjectDomain>(new PaginationOptions(options, count), mapper.Map<IEnumerable<ProjectDomain>>(data));
        }

        public async override Task<ProjectDomain?> GetByIdAsync(string id, CancellationToken cancellationToken = default) => await GetByIdAsync(new ProjectEntity(id), cancellationToken);

        private async Task<ProjectDomain?> GetByIdAsync(ProjectEntity entity, CancellationToken cancellationToken = default)
        {
            var filter = new AndFilter(
                new PropertyEqualsFilter(nameof(ITableEntity.PartitionKey), entity.PartitionKey),
                new PropertyEqualsFilter(nameof(ITableEntity.RowKey), entity.RowKey)
                );

            var results = client.Query<ProjectEntity>(filter.ToAzureFilterString(), null, null, cancellationToken).FirstOrDefault();

            return await Task.FromResult(results is null ? null : mapper.Map<ProjectDomain>(results));
        }

        public override async Task<bool?> UpdateAsync(ProjectDomain domain, CancellationToken cancellationToken = default)
        {
            var entity = mapper.Map<ProjectEntity>(domain);
            try
            {
                await client.UpdateEntityAsync(entity, default, TableUpdateMode.Replace, cancellationToken);
                return true;
            }
            catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.NoContent)
            {
                return false;
            }
            catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.NotFound)
            {
                return null;
            }
            catch (Exception) // on sait jamais, faudra revoir ca un jour ....
            {
                return null;
            }
        }
    }
}
