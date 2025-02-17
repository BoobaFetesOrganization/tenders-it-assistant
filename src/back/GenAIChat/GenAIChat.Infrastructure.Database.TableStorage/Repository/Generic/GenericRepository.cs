using Azure.Data.Tables;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using System.Linq.Expressions;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository.Generic
{
    public abstract class GenericRepository<TEntity> : IRepositoryAdapter<TEntity> where TEntity : class, IEntityDomain, ITableEntity
    {
        protected TableClient client;

        protected GenericRepository(TableServiceClient service, string tableName)
        {
            client = service.GetTableClient(tableName);
            client.CreateIfNotExistsAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            int count = 0;
            int maxPerPage = 1000;
            string? continuationToken = null;

            var response = client.Query<TEntity>((entity) => true, maxPerPage, ["PartitionKey", "RowKey"]); // Ajustez maxPerPage
            foreach (var page in response.AsPages(continuationToken))
            {
                count += page.Values.Count;
                continuationToken = page.ContinuationToken;
            }

            return await Task.FromResult(count);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(PaginationOptions options, Expression<Func<TEntity, bool>>? filter = null)
        {
            // arrange
            Expression<Func<TEntity, bool>> _filter = filter ?? (entity => true);


            return await Task.Run(() =>
            {
                var response = client.Query(_filter, options.Limit);
                int offset = 0;
                var result = new List<TEntity>();
                foreach (var page in response.AsPages())
                {
                    // skip until options.Offset is reached
                    offset += page.Values.Count;
                    if (options.Offset > offset) continue;

                    result.AddRange(page.Values);
                }
                return result;
            });
        }

        public async Task<TEntity?> GetByIdAsync(string id) => await Task.FromResult(client.Query<TEntity>(filter: $"PartitionKey eq '{id}'").FirstOrDefault());

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var response = await client.AddEntityAsync(entity);

            var entry = await client.GetEntityAsync<TEntity>(entity.PartitionKey, (entity as ITableEntity).RowKey);
            return entry;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var response = await client.UpdateEntityAsync(entity, Azure.ETag.All);

            var entry = await client.GetEntityAsync<TEntity>(entity.PartitionKey, (entity as ITableEntity).RowKey);
            return entry;
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await client.DeleteEntityAsync(entity);
        }

        public Task SaveAsync(CancellationToken cancellationToken = default)
        {
            // TableStorage is a NoSQL database, so there is no need to save changes, it is already done.
            return Task.CompletedTask;
        }

        #region implements IDisposable
        private bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // Libérer les ressources managées
                // nothing to dispose
            }

            // Libérer les ressources non managées si nécessaire
            _disposed = true;
        }

        ~GenericRepository()
        {
            Dispose(false);
        }
        #endregion
    }
}
