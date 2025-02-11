using GenAIChat.Application.Adapter.Database.Repository;
using GenAIChat.Domain.Project.Group.UserStory;
using GenAIChat.Infrastructure.Database.Sqlite.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace GenAIChat.Infrastructure.Database.Sqlite.Repository
{
    public class UserStoryRepository(GenAiDbContext dbContext) : GenericRepository<UserStoryDomain>(dbContext), IUserStoryRepositoryAdapter
    {
        protected override IQueryable<UserStoryDomain> GetPropertiesForCollection(IQueryable<UserStoryDomain> query) => query.Include(i => i.Tasks);
        protected override IQueryable<UserStoryDomain> GetProperties(IQueryable<UserStoryDomain> query) => query
            .Include(us => us.Tasks)
                .ThenInclude(t => t.WorkingCosts);

    }
}
