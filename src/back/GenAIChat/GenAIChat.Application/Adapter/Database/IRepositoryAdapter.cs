using GenAIChat.Domain.Common;
using System.Linq.Expressions;

namespace GenAIChat.Application.Adapter.Database
{
    public interface IRepositoryAdapter<TEntity> : IDisposable where TEntity : class, IEntityDomain
    {
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null);
        Task<Paged<TEntity>> GetAllPagedAsync(PaginationOptions options, Expression<Func<TEntity, bool>>? filter = null);
        Task<TEntity?> GetByIdAsync(string id);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<TEntity> DeleteAsync(TEntity entity);
        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}

