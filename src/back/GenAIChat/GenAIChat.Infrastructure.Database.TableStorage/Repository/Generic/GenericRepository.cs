using AutoMapper;
using Azure;
using Azure.Data.Tables;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Specifications;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Infrastructure.Database.TableStorage.Entity.Common;
using GenAIChat.Infrastructure.Database.TableStorage.Extensions;
using System.Net;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository.Generic
{
    internal interface IGenericRepository<TDomain, TEntity> : IRepositoryAdapter<TDomain>
        where TDomain : class, IEntityDomain
        where TEntity : BaseEntity, new()
    {
        public Task<TEntity?> GetEntityByIdAsync(string id, CancellationToken cancellationToken = default);

    }
    internal abstract class GenericRepository<TDomain, TEntity> : IGenericRepository<TDomain, TEntity>
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

        public async virtual Task<bool> DeleteAsync(TDomain domain, CancellationToken cancellationToken = default)
        {
            var entity = mapper.Map<TEntity>(domain);
            try
            {
                await client.DeleteEntityAsync(entity.PartitionKey, entity.RowKey, default, cancellationToken);
            }
            catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.NoContent)
            {
                return false;
            }
            catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.NotFound)
            {
                return false;
            }
            catch (Exception) // on sait jamais, faudra revoir ca un jour ....
            {
                return false;
            }
            return true;
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

        public async Task<TEntity?> GetEntityByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var (partitionKey, rowKey) = TableStorageTools.ExtractKeys(id);
            var filter = new AndFilter(
                new PropertyEqualsFilter(nameof(ITableEntity.PartitionKey), partitionKey),
                new PropertyEqualsFilter(nameof(ITableEntity.RowKey), rowKey)
                ).ToAzureFilterString();

            var data = await Task.Run(
                () => client.Query<TEntity>(filter, 1, null, cancellationToken).FirstOrDefault()
                , cancellationToken);

            return data;
        }
        public async virtual Task<TDomain?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var data = await GetEntityByIdAsync(id, cancellationToken);
            return data is null ? null : mapper.Map<TDomain>(data);
        }

        private static readonly ICollection<string> UpdateIgnoredProperties = [nameof(ITableEntity.Timestamp), nameof(ITableEntity.ETag)];
        public async virtual Task<bool> UpdateAsync(TDomain domain, CancellationToken cancellationToken = default)
        {
            // ACT 1 : COMPARE DOMAIN AND DATABASE ENTITY TO DETERMINE IF AN UPDATE IS NECESSARY
            var entity = mapper.Map<TEntity>(domain);
            var stored = await GetEntityByIdAsync(domain.Id, cancellationToken);

            // if the entity does not exist, we consider the operation a failure
            if (stored is null) return false;
            
            // if the two objects are identical, we consider the operation a success
            if (entity.AreShallowSameAs(stored, UpdateIgnoredProperties)) return true;

            // ACT 2 : UPDATE THE ENTITY AND PRESERVE THE ETAG

            // affect new values but preserve the etag            
            var etag = stored.ETag;
            mapper.Map(entity, stored); // merge !
            stored.ETag = etag; // restore etag

            try
            {
                // NOTE : here we can try to use a batch operation, in a later version !
                await client.UpdateEntityAsync(stored, stored.ETag, TableUpdateMode.Replace, cancellationToken);
                return true;
            }
            catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.NoContent)
            {
                return false;
            }
            catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.NotFound)
            {
                return false;
            }
            catch (Exception) // on sait jamais, faudra revoir ca un jour ....
            {
                return false;
            }
        }

        public Task SaveAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public void Dispose()
        {

            GC.SuppressFinalize(this);
        }
    }
}
