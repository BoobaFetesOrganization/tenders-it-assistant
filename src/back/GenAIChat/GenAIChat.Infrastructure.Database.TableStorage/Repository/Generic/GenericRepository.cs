using AutoMapper;
using Azure;
using Azure.Data.Tables;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Specifications;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Infrastructure.Database.TableStorage.Entity.Common;
using System.Net;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository.Generic
{
    internal abstract class GenericRepository<TDomain, TEntity> : IRepositoryAdapter<TDomain>
        where TDomain : class, IEntityDomain
        where TEntity : BaseEntity, new()
    {
        protected TableClient client;
        protected IMapper mapper;

        public GenericRepository(TableServiceClient service, string tableName, IMapper mapper)
        {
            client = service.GetTableClient(tableName);
            client.CreateIfNotExistsAsync();
            this.mapper = mapper;
        }

        public async virtual Task<TDomain> AddAsync(TDomain domain, CancellationToken cancellationToken = default)
        {
            var entity = mapper.Map<TEntity>(domain);
            await client.AddEntityAsync(entity, cancellationToken);
            var result = await GetByIdAsync(entity.Id, cancellationToken);
            return result!;
        }

        public async virtual Task<int> CountAsync(IFilter? filter = null, CancellationToken cancellationToken = default)
        {
            var filterString = filter?.ToAzureFilterString();
            var count = client.Query<TEntity>(filterString, null, null, cancellationToken).Count();
            return await Task.FromResult(count);
        }

        public async virtual Task<bool?> DeleteAsync(TDomain domain, CancellationToken cancellationToken = default)
        {
            var entity = mapper.Map<TEntity>(domain);
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

        public async virtual Task<IEnumerable<TDomain>> GetAllAsync(IFilter? filter = null, CancellationToken cancellationToken = default)
        {
            var results = client.Query<TEntity>(filter?.ToAzureFilterString(), null, null, cancellationToken).ToArray();
            return await Task.FromResult(mapper.Map<IEnumerable<TDomain>>(results));
        }

        public async virtual Task<Paged<TDomain>> GetAllPagedAsync(PaginationOptions options, IFilter? filter = null, CancellationToken cancellationToken = default)
        {
            var filterString = filter?.ToAzureFilterString();

            // A single request is made to retrieve the desired page and does not block the current thread
            return await Task.Run(async () =>
            {
                var count = await CountAsync(filter, cancellationToken);
                var data = client.Query<TEntity>(filterString, null, null, cancellationToken)
                    .Skip(options.Offset)
                    .Take(options.Limit).ToArray();

                return new Paged<TDomain>(
                    new PaginationOptions(options, count),
                    mapper.Map<IEnumerable<TDomain>>(data));
            });
        }

        public async virtual Task<TDomain?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var (partitionKey, rowKey) = TableStorageTools.ExtractKeys(id);
            var filter = new AndFilter(
                new PropertyEqualsFilter(nameof(ITableEntity.PartitionKey), partitionKey),
                new PropertyEqualsFilter(nameof(ITableEntity.RowKey), rowKey)
                ).ToAzureFilterString();

            var data = await Task.Run(
                () => client.Query<TEntity>(filter, 1, null, cancellationToken).FirstOrDefault()
                , cancellationToken);
            return data is null ? null : mapper.Map<TDomain>(data);
        }

        public async virtual Task<bool?> UpdateAsync(TDomain domain, CancellationToken cancellationToken = default)
        {
            var entity = mapper.Map<TEntity>(domain);
            try
            {
                await client.UpdateEntityAsync(entity, new ETag("*"), TableUpdateMode.Replace, cancellationToken);
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
        public Task SaveAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public void Dispose()
        {

            GC.SuppressFinalize(this);
        }
    }
}
