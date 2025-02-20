using Azure.Data.Tables;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Text.Json;

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

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            Expression<Func<TEntity, bool>> _filter = filter ?? (entity => true);

            var response = client.Query(_filter);

            return await Task.FromResult(response.ToImmutableArray());
        }

        public async Task<Paged<TEntity>> GetAllPagedAsync(PaginationOptions options, Expression<Func<TEntity, bool>>? filter = null)
        {
            Expression<Func<TEntity, bool>> _filter = filter ?? (entity => true);

            // act : query
            var response = client.Query(_filter, options.Limit);

            // act : paginate
            string? continuationToken = null;
            List<TEntity> results = [];

            int count = 0;
            foreach (var page in response.AsPages(continuationToken))
            {
                count += page.Values.Count;
                continuationToken = page.ContinuationToken;

                // check 
                if (count != options.Offset) continue;

                // act
                results.AddRange(page.Values);
            }

            return await Task.FromResult(new Paged<TEntity>(new PaginationOptions(options, count), results));
        }

        public async Task<TEntity?> GetByIdAsync(string id) => await Task.FromResult(client.Query<TEntity>(filter: $"PartitionKey eq '{id}'").FirstOrDefault());

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (string.IsNullOrEmpty(entity.PartitionKey)) throw new ArgumentException("PartitionKey has to be set");
            if (string.IsNullOrEmpty(entity.RowKey)) throw new ArgumentException("RowKey has to be set");

            var json = JsonSerializer.Serialize(entity);
            Console.WriteLine("entity as json");
            Console.WriteLine(json);

            // Ajoutez des journaux pour vérifier les valeurs des propriétés de l'entité
            Console.WriteLine($"Adding entity with PartitionKey: {entity.PartitionKey}, RowKey: {entity.RowKey}");

            // Vérifiez les autres propriétés de l'entité si nécessaire
            foreach (var property in typeof(TEntity).GetProperties())
            {
                var value = property.GetValue(entity);
                Console.WriteLine($"{property.Name}: {value}");
            }

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

        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            await client.DeleteEntityAsync(entity);
            return entity;
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
