using GenAIChat.Application.Adapter.Database.Generic;
using GenAIChat.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GenAIChat.Infrastructure.Database.Repository.Generic
{
    public abstract class GenericRepository<TEntity> : IRepositoryAdapter<TEntity> where TEntity : class, IEntityDomain
    {
        private readonly GenAiDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(GenAiDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(PaginationOptions options, Expression<Func<TEntity, bool>>? filter = null)
        {
            IQueryable<TEntity> query = GetPropertiesForCollection(_dbSet);

            query = query.Skip(options.Offset);
            if (options.Limit.HasValue) query = query.Take(options.Limit.Value);

            if (filter is not null) query = query.Where(filter);

            return await query.ToListAsync();
        }

        protected virtual IQueryable<TEntity> GetPropertiesForCollection(IQueryable<TEntity> query) => query;
        protected virtual IQueryable<TEntity> GetProperties(IQueryable<TEntity> query) => query;

        public async Task<TEntity?> GetByIdAsync(int id) => await GetProperties(_dbSet).FirstOrDefaultAsync(i => i.Id == id);

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var entry = await _dbSet.AddAsync(entity);
            return entry.Entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var entry = _dbSet.Update(entity);
            return await Task.FromResult(entry.Entity);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
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
                _dbContext.Dispose();
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
