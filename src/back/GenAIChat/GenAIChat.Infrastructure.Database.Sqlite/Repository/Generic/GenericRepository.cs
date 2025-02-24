using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Infrastructure.Database.Sqlite.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GenAIChat.Infrastructure.Database.Sqlite.Repository.Generic
{
    public abstract class GenericRepository<TDomain>(GenAiDbContext dbContext) : IRepositoryAdapter<TDomain> where TDomain : class, IEntityDomain
    {
        private readonly DbSet<TDomain> _dbSet = dbContext.Set<TDomain>();

        protected virtual IQueryable<TDomain> GetPropertiesForCollection(IQueryable<TDomain> query) => query;
        protected virtual IQueryable<TDomain> GetProperties(IQueryable<TDomain> query) => query;

        public async Task<int> CountAsync(Expression<Func<TDomain, bool>>? filter = null)
        {
            IQueryable<TDomain> query = _dbSet;
            if (filter is not null) query = query.Where(filter);
            return await query.CountAsync();
        }

        public Task<IEnumerable<TDomain>> GetAllAsync2(IFilter? specifications = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TDomain>> GetAllAsync(Expression<Func<TDomain, bool>>? filter = null) => await GetAllWhereAsQuery(filter).ToListAsync();

        private IQueryable<TDomain> GetAllWhereAsQuery(Expression<Func<TDomain, bool>>? filter = null)
        {
            IQueryable<TDomain> query = GetPropertiesForCollection(_dbSet);

            if (filter is not null) query = query.Where(filter);

            return query;
        }

        public async Task<Paged<TDomain>> GetAllPagedAsync(PaginationOptions options, Expression<Func<TDomain, bool>>? filter = null)
        {
            // arrange
            var countRequest = CountAsync(filter);
            IQueryable<TDomain> query = GetAllWhereAsQuery(filter);

            query = query.Skip(options.Offset);
            query = query.Take(options.Limit);

            // act
            var data = await query.ToArrayAsync();
            var count = await countRequest;
            return new Paged<TDomain>(new PaginationOptions(options, count), data);
        }

        public async Task<TDomain?> GetByIdAsync(string id) => await GetProperties(_dbSet).FirstOrDefaultAsync(i => i.Id == id);

        public async Task<TDomain> AddAsync(TDomain entity)
        {
            entity.SetNewTimeStamp();
            var entry = await _dbSet.AddAsync(entity);
            await SaveAsync();
            return entry.Entity;
        }

        public async Task<TDomain> UpdateAsync(TDomain entity)
        {
            var actual = _dbSet.Find(entity.Id) ?? throw new InvalidOperationException($"entity not exists");
            if (actual.Timestamp != entity.Timestamp) throw new InvalidOperationException($"Timestamp is not up to date, expected : '{actual.Timestamp} but has '{entity.Timestamp}'");

            entity.SetNewTimeStamp();
            var entry = _dbSet.Update(entity);
            await SaveAsync();
            return entry.Entity;
        }

        public async Task<TDomain> DeleteAsync(TDomain entity)
        {
            var actual = _dbSet.Find(entity.Id) ?? throw new InvalidOperationException($"entity not exists");
            if (actual.Timestamp != entity.Timestamp) throw new InvalidOperationException($"Timestamp is not up to date, expected : '{actual.Timestamp} but has '{entity.Timestamp}'");

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
