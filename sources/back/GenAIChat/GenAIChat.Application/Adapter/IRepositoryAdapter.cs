using GenAIChat.Domain.Common;
using System.Linq.Expressions;

namespace GenAIChat.Infrastructure.Database.Repository.Generic
{
    public interface IRepositoryAdapter<TEntity> where TEntity : class, IEntityDomain
    {
        Task<Paged<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null);
        Task<Paged<TEntity>> GetAllAsync(PaginationOptions options, Expression<Func<TEntity, bool>>? filter = null);
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity> AddAsync(TEntity entity);
        TEntity Update(TEntity entity);
        void Delete(TEntity entity);
    }
}

