using GenAIChat.Application.Adapter.Database.Repository;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Infrastructure.Database.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace GenAIChat.Infrastructure.Database.Repository
{
    public class UserStoryGroupRepository(GenAiDbContext dbContext) : GenericRepository<UserStoryGroupDomain>(dbContext), IUserStoryGroupRepositoryAdapter
    {
        protected override IQueryable<UserStoryGroupDomain> GetProperties(IQueryable<UserStoryGroupDomain> query) => GetPropertiesForCollection(query)
            .Include(i => i.Request)
            .Include(i => i.UserStories)
                .ThenInclude(us => us.Tasks)
                .ThenInclude(t => t.WorkingCosts);// récuperation pour setter la proprieté Cost;

    }
}
