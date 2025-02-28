using AutoMapper;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Infrastructure.Database.Sqlite.Extensions;
using Microsoft.EntityFrameworkCore;

namespace GenAIChat.Infrastructure.Database.Sqlite.Repository.Generic
{
    public abstract class GenericRepository<TDomain>(GenAiDbContext dbContext, IMapper mapper) : IRepositoryAdapter<TDomain> where TDomain : class, IEntityDomain
    {
        private readonly DbSet<TDomain> _dbSet = dbContext.Set<TDomain>();

        protected virtual IQueryable<TDomain> GetPropertiesForCollection(IQueryable<TDomain> query) => query;
        protected virtual IQueryable<TDomain> GetProperties(IQueryable<TDomain> query) => query;

        public async Task<int> CountAsync(IFilter? filter = null) => await GetAllWhereAsQuery(filter).CountAsync();

        public async Task<IEnumerable<TDomain>> GetAllAsync(IFilter? filter = null) => await GetAllWhereAsQuery(filter).ToListAsync();

        public async Task<Paged<TDomain>> GetAllPagedAsync(PaginationOptions options, IFilter? filter = null)
        {
            // arrange
            var countRequest = CountAsync(filter);
            IQueryable<TDomain> query = GetAllWhereAsQuery(filter)
                .Skip(options.Offset)
                .Take(options.Limit);

            // act
            var data = await query.ToArrayAsync();
            var count = await countRequest;
            return new Paged<TDomain>(new PaginationOptions(options, count), data);
        }

        private IQueryable<TDomain> GetAllWhereAsQuery(IFilter? filter = null)
        {
            IQueryable<TDomain> query = GetPropertiesForCollection(_dbSet);

            if (filter is not null) query = query.Where(filter.ToQueryableExpression<TDomain>());

            return query.AsNoTracking();
        }

        public async Task<TDomain?> GetByIdAsync(string id) => await GetProperties(_dbSet.AsNoTracking()).FirstOrDefaultAsync(e => e.Id == id);

        public async Task<TDomain> AddAsync(TDomain domain)
        {
            domain.SetNewTimeStamp();

            await _dbSet.AddAsync(domain);
            await SaveAsync();

            dbContext.Detach(domain);
            return domain;
        }

        public async Task<bool?> UpdateAsync(TDomain domain)
        {
            TDomain actual = _dbSet.Find(domain.Id) ?? throw new InvalidOperationException($"entity not exists");
            if (actual.Timestamp != domain.Timestamp) throw new InvalidOperationException($"Timestamp is not up to date, expected : '{actual.Timestamp} but has '{domain.Timestamp}'");

            // attention: le merge est effectué avec automapper.
            // De plus, EF Core impose que l'on donne à_dbSet.Update l'entité trackée afin qu'il puisse effectuer les mofications automatiquement en base de données
            // DONC : le mapping est très important, hors il est chargé à l'initialisation, donc attention aux effet de bord qui pourrait survenir dans la couche Application ou Presentation !

            // merge domain in 'actual' and change the timestamp
            mapper.Map(domain, actual);
            actual.SetNewTimeStamp();

            _dbSet.Update(actual);
            await SaveAsync();

            dbContext.Detach(actual);
            return true;
        }

        public async Task<bool?> DeleteAsync(TDomain domain)
        {
            var actual = _dbSet.Find(domain.Id) ?? throw new InvalidOperationException($"entity not exists");
            if (actual.Timestamp != domain.Timestamp) throw new InvalidOperationException($"Timestamp is not up to date, expected : '{actual.Timestamp} but has '{domain.Timestamp}'");

            _dbSet.Remove(actual);
            await SaveAsync();
            dbContext.Detach(actual);

            return true;
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
