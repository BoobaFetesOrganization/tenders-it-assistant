using GenAIChat.Domain.Common;
using System.Linq.Expressions;

namespace GenAIChat.Application.Adapter.Database.Generic
{
    public interface IRepositoryAdapter<TEntity> : IDisposable where TEntity : class, IEntityDomain
    {
        public Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null);
        Task<IEnumerable<TEntity>> GetAllAsync(PaginationOptions options, Expression<Func<TEntity, bool>>? filter = null);
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}

