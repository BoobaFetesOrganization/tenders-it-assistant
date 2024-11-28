using GenAIChat.Application.Adapter.Database.Repository;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Infrastructure.Database.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace GenAIChat.Infrastructure.Database.Repository
{
    public class UserStoryGroupRepository(GenAiDbContext dbContext) : GenericRepository<UserStoryGroupDomain>(dbContext), IUserStoryGroupRepositoryAdapter
    {
        protected override IQueryable<UserStoryGroupDomain> GetPropertiesForCollection(IQueryable<UserStoryGroupDomain> query) => query
            .Include(i => i.UserStories)
                .ThenInclude(us => us.Tasks);// récuperation pour setter la proprieté Cost
        protected override IQueryable<UserStoryGroupDomain> GetProperties(IQueryable<UserStoryGroupDomain> query) => query
            .Include(i => i.UserStories)
                .ThenInclude(us => us.Tasks);

    }
}
