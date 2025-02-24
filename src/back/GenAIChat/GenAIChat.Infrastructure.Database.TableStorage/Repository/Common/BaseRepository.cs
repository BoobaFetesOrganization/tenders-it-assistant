using Azure.Data.Tables;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using System.Linq.Expressions;

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

        public abstract Task<TDomain> AddAsync(TDomain domain);
        public abstract Task<int> CountAsync(Expression<Func<TDomain, bool>>? filter = null);
        public abstract Task<TDomain> DeleteAsync(TDomain domain);
        public abstract Task<IEnumerable<TDomain>> GetAllAsync2(IFilter? filter = null);
        public abstract Task<IEnumerable<TDomain>> GetAllAsync(Expression<Func<TDomain, bool>>? filter = null);
        public abstract Task<Paged<TDomain>> GetAllPagedAsync(PaginationOptions options, Expression<Func<TDomain, bool>>? filter = null);
        public abstract Task<TDomain?> GetByIdAsync(string id);
        public abstract Task<TDomain> UpdateAsync(TDomain domain);
        public Task SaveAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
