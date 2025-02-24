//using AutoMapper;
//using Azure.Data.Tables;
//using GenAIChat.Application.Adapter.Database;
//using GenAIChat.Domain.Common;
//using System.Collections.Immutable;
//using System.Linq.Expressions;
//using System.Text.Json;

//namespace GenAIChat.Infrastructure.Database.TableStorage.Repository.Generic
//{
//    public abstract class GenericRepository<TDomain> : IRepositoryAdapter<TDomain> where TDomain : class, IEntityDomain
//    {
//        protected TableClient client;
//        protected IMapper mapper;

//        protected GenericRepository(TableServiceClient service, string tableName, IMapper mapper)
//        {
//            client = service.GetTableClient(tableName);
//            client.CreateIfNotExistsAsync();
//            this.mapper = mapper;
//        }

//        public async Task<int> CountAsync(Expression<Func<TDomain, bool>>? filter = null)
//        {
//            int count = 0;
//            int maxPerPage = 1000;
//            string? continuationToken = null;

//            var response = client.Query<TDomain>((entity) => true, maxPerPage, ["PartitionKey", "RowKey"]); // Ajustez maxPerPage
//            foreach (var page in response.AsPages(continuationToken))
//            {
//                count += page.Values.Count;
//                continuationToken = page.ContinuationToken;
//            }

//            return await Task.FromResult(count);
//        }

//        public async Task<IEnumerable<TDomain>> GetAllAsync(Expression<Func<TDomain, bool>>? filter = null)
//        {
//            Expression<Func<TDomain, bool>> _filter = filter ?? (entity => true);

//            var response = client.Query(_filter);

//            return await Task.FromResult(response.ToImmutableArray());
//        }

//        public async Task<Paged<TDomain>> GetAllPagedAsync(PaginationOptions options, Expression<Func<TDomain, bool>>? filter = null)
//        {
//            Expression<Func<TDomain, bool>> _filter = filter ?? (entity => true);

//            // act : query
//            var response = client.Query(_filter, options.Limit);

//            // act : paginate
//            string? continuationToken = null;
//            List<TDomain> results = [];

//            int count = 0;
//            foreach (var page in response.AsPages(continuationToken))
//            {
//                count += page.Values.Count;
//                continuationToken = page.ContinuationToken;

//                // check 
//                if (count != options.Offset) continue;

//                // act
//                results.AddRange(page.Values);
//            }

//            return await Task.FromResult(new Paged<TDomain>(new PaginationOptions(options, count), results));
//        }

//        public async Task<TDomain?> GetByIdAsync(string id) => await Task.FromResult(client.Query<TDomain>(filter: $"PartitionKey eq '{id}'").FirstOrDefault());

//        public async Task<TDomain> AddAsync(TDomain entity)
//        {
//            if (string.IsNullOrEmpty(entity.PartitionKey)) throw new ArgumentException("PartitionKey has to be set");
//            if (string.IsNullOrEmpty(entity.RowKey)) throw new ArgumentException("RowKey has to be set");

//            var json = JsonSerializer.Serialize(entity);
//            Console.WriteLine("entity as json");
//            Console.WriteLine(json);

//            // Ajoutez des journaux pour vérifier les valeurs des propriétés de l'entité
//            Console.WriteLine($"Adding entity with PartitionKey: {entity.PartitionKey}, RowKey: {entity.RowKey}");

//            // Vérifiez les autres propriétés de l'entité si nécessaire
//            foreach (var property in typeof(TDomain).GetProperties())
//            {
//                var value = property.GetValue(entity);
//                Console.WriteLine($"{property.Name}: {value}");
//            }

//            var response = await client.AddEntityAsync(entity);

//            var entry = await client.GetEntityAsync<TDomain>(entity.PartitionKey, (entity as ITableEntity).RowKey);
//            return entry;
//        }

//        public async Task<TDomain> UpdateAsync(TDomain entity)
//        {
//            var response = await client.UpdateEntityAsync(entity, Azure.ETag.All);

//            var entry = await client.GetEntityAsync<TDomain>(entity.PartitionKey, (entity as ITableEntity).RowKey);
//            return entry;
//        }

//        public async Task<TDomain> DeleteAsync(TDomain entity)
//        {
//            await client.DeleteEntityAsync(entity);
//            return entity;
//        }

//        public Task SaveAsync(CancellationToken cancellationToken = default)
//        {
//            // TableStorage is a NoSQL database, so there is no need to save changes, it is already done.
//            return Task.CompletedTask;
//        }

//        #region implements IDisposable
//        private bool _disposed = false;
//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        protected virtual void Dispose(bool disposing)
//        {
//            if (_disposed) return;

//            if (disposing)
//            {
//                // Libérer les ressources managées
//                // nothing to dispose
//            }

//            // Libérer les ressources non managées si nécessaire
//            _disposed = true;
//        }

//        ~GenericRepository()
//        {
//            Dispose(false);
//        }
//        #endregion
//    }
//}
