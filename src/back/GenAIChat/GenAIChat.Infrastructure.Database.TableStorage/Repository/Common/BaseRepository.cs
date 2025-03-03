using Azure.Data.Tables;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository.Common
{
    public abstract class BaseRepository<TDomain> : IRepositoryAdapter<TDomain> where TDomain : class, IEntityDomain
    {
        protected TableClient client;

        protected BaseRepository(TableServiceClient service, string tableName)
        {
            client = service.GetTableClient(tableName);
            client.CreateIfNotExistsAsync();
        }

        public abstract Task<TDomain> AddAsync(TDomain domain, CancellationToken cancellationToken = default);
        public abstract Task<int> CountAsync(IFilter? filter = null, CancellationToken cancellationToken = default);
        public abstract Task<bool?> DeleteAsync(TDomain domain, CancellationToken cancellationToken = default);
        public abstract Task<IEnumerable<TDomain>> GetAllAsync(IFilter? filter = null, CancellationToken cancellationToken = default);
        public abstract Task<Paged<TDomain>> GetAllPagedAsync(PaginationOptions options, IFilter? filter = null, CancellationToken cancellationToken = default);
        public abstract Task<TDomain?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        public abstract Task<bool?> UpdateAsync(TDomain domain, CancellationToken cancellationToken = default);
        public Task SaveAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
