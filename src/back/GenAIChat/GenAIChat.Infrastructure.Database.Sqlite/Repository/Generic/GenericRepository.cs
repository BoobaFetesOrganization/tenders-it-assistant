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

        public async Task<IEnumerable<TDomain>> GetAllAsync2(IFilter? filter = null)
        {
            var queryableFilter = filter?.ToQueryableExpression<TDomain>();
            IQueryable<TDomain> query = GetPropertiesForCollection(_dbSet);

            if (queryableFilter is not null) query = query.Where(queryableFilter);

            return await query.ToListAsync();
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

        public async Task<TDomain> AddAsync(TDomain domain)
        {
            var entity = (TDomain)domain.Clone();
            entity.Id = EntityDomain.NewId();
            entity.SetNewTimeStamp();

            var entry = await _dbSet.AddAsync(entity);
            await SaveAsync();
            return entry.Entity;
        }

        public async Task<TDomain> UpdateAsync(TDomain domain)
        {
            var actual = _dbSet.Find(domain.Id) ?? throw new InvalidOperationException($"entity not exists");
            if (actual.Timestamp != domain.Timestamp) throw new InvalidOperationException($"Timestamp is not up to date, expected : '{actual.Timestamp} but has '{domain.Timestamp}'");

            domain.SetNewTimeStamp();
            var entry = _dbSet.Update(domain);
            await SaveAsync();
            return entry.Entity;
        }

        public async Task<TDomain> DeleteAsync(TDomain domain)
        {
            var actual = _dbSet.Find(domain.Id) ?? throw new InvalidOperationException($"entity not exists");
            if (actual.Timestamp != domain.Timestamp) throw new InvalidOperationException($"Timestamp is not up to date, expected : '{actual.Timestamp} but has '{domain.Timestamp}'");

            domain.SetNewTimeStamp();
            _dbSet.Remove(domain);
            await SaveAsync();

            return domain;
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
