using GenAIChat.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GenAIChat.Infrastructure.Database.Repository.Generic
{
    public class GenericRepository<TEntity> : IRepositoryAdapter<TEntity> where TEntity : class, IEntityDomain
    {
        private readonly GenAiDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(GenAiDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<Paged<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await GetAllAsync(new PaginationOptions(), filter);
        }
        public async Task<Paged<TEntity>> GetAllAsync(PaginationOptions options, Expression<Func<TEntity, bool>>? filter = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null) query = query.Where(filter);

            query = query.Skip(options.Offset).Take(options.Limit);

            return new Paged<TEntity>(options, await query.ToListAsync());
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var entry = await _dbSet.AddAsync(entity);
            return entry.Entity;
        }

        public TEntity Update(TEntity entity)
        {
            var entry = _dbSet.Update(entity);
            return entry.Entity;
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
