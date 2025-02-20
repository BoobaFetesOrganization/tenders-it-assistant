using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using GenAIChat.Infrastructure.Database.Sqlite.Extensions;
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

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null) => await GetAllWhereAsQuery(filter).ToListAsync();

        private IQueryable<TEntity> GetAllWhereAsQuery(Expression<Func<TEntity, bool>>? filter = null)
        {
            IQueryable<TEntity> query = GetPropertiesForCollection(_dbSet);

            if (filter is not null) query = query.Where(filter);

            return query;
        }

        public async Task<Paged<TEntity>> GetAllPagedAsync(PaginationOptions options, Expression<Func<TEntity, bool>>? filter = null)
        {
            // arrange
            var countRequest = CountAsync(filter);
            IQueryable<TEntity> query = GetAllWhereAsQuery(filter);

            query = query.Skip(options.Offset);
            query = query.Take(options.Limit);

            // act
            var data = await query.ToArrayAsync();
            var count = await countRequest;
            return new Paged<TEntity>(new PaginationOptions(options, count), data);
        }

        public async Task<TEntity?> GetByIdAsync(string id) => await GetProperties(_dbSet).FirstOrDefaultAsync(i => i.Id == id);

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            entity.SetNewTimeStamp();
            var entry = await _dbSet.AddAsync(entity);
            await SaveAsync();
            return entry.Entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var actual = _dbSet.Find(entity.Id) ?? throw new InvalidOperationException($"entity not exists");
            if (actual.Timestamp != entity.Timestamp) throw new InvalidOperationException($"Timestamp is not up to date, expected : '{actual.RowKey} but has '{entity.RowKey}'");

            entity.SetNewTimeStamp();
            var entry = _dbSet.Update(entity);
            await SaveAsync();
            return entry.Entity;
        }

        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            var actual = _dbSet.Find(entity.Id) ?? throw new InvalidOperationException($"entity not exists");
            if (actual.RowKey != entity.RowKey) throw new InvalidOperationException($"RowKey is not up to date, expected : '{actual.RowKey} but has '{entity.RowKey}'");

            entity.SetNewTimeStamp();
            _dbSet.Remove(entity);
            await SaveAsync();

            return entity;
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
