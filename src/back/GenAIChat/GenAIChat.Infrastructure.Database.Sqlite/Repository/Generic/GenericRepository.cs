using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GenAIChat.Infrastructure.Database.Sqlite.Repository.Generic
{
    public abstract class GenericRepository<TEntity>(GenAiDbContext dbContext) : IRepositoryAdapter<TEntity> where TEntity : class, IEntityDomain
    {
        private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

        protected virtual IQueryable<TEntity> GetPropertiesForCollection(IQueryable<TEntity> query) => query;
        protected virtual IQueryable<TEntity> GetProperties(IQueryable<TEntity> query) => query;

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (filter is not null) query = query.Where(filter);
            return await query.CountAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(PaginationOptions options, Expression<Func<TEntity, bool>>? filter = null)
        {
            IQueryable<TEntity> query = GetPropertiesForCollection(_dbSet);

            if (filter is not null) query = query.Where(filter);

            query = query.Skip(options.Offset);
            if (options.Limit.HasValue) query = query.Take(options.Limit.Value);

            return await query.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(string id) => await GetProperties(_dbSet).FirstOrDefaultAsync(i => i.Id == id);

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var entry = await _dbSet.AddAsync(entity);
            await SaveAsync();
            return entry.Entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var actual = _dbSet.Find(entity.Id) ?? throw new InvalidOperationException($"entity not exists");
            if (actual.Timestamp != entity.Timestamp) throw new InvalidOperationException($"Timestamp is not up to date, expected : '{actual.RowKey} but has '{entity.RowKey}'");

            EntityDomain.SetNewTimeStamp(entity);
            var entry = _dbSet.Update(entity);
            await SaveAsync();
            return entry.Entity;
        }

        public async Task DeleteAsync(TEntity entity)
        {
            var actual = _dbSet.Find(entity.Id) ?? throw new InvalidOperationException($"entity not exists");
            if (actual.RowKey != entity.RowKey) throw new InvalidOperationException($"RowKey is not up to date, expected : '{actual.RowKey} but has '{entity.RowKey}'");

            EntityDomain.SetNewTimeStamp(entity);
            _dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
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
                dbContext.Dispose();
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
