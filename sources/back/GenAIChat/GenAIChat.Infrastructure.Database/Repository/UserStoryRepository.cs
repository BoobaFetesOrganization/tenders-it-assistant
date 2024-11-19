using GenAIChat.Application.Adapter.Database.Repository;
using GenAIChat.Domain.Project;
using GenAIChat.Infrastructure.Database.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace GenAIChat.Infrastructure.Database.Repository
{
    public class UserStoryRepository : GenericRepository<UserStoryDomain>, IUserStoryRepositoryAdapter
    {
        public UserStoryRepository(GenAiDbContext dbContext) : base(dbContext)
        {
        }

        protected override IQueryable<UserStoryDomain> GetPropertiesForCollection(IQueryable<UserStoryDomain> query) => query.Include(i => i.Tasks);
        protected override IQueryable<UserStoryDomain> GetProperties(IQueryable<UserStoryDomain> query) => query.Include(i => i.Tasks);

    }
}
