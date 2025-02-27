using AutoMapper;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Infrastructure.Database.Sqlite.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace GenAIChat.Infrastructure.Database.Sqlite.Repository
{
    public class UserStoryGroupRepository(GenAiDbContext dbContext, IMapper mapper) : GenericRepository<UserStoryGroupDomain>(dbContext, mapper)
    {
        protected override IQueryable<UserStoryGroupDomain> GetProperties(IQueryable<UserStoryGroupDomain> query) => GetPropertiesForCollection(query)
            .Include(i => i.Request)
            .Include(i => i.UserStories)
                .ThenInclude(us => us.Tasks)
                .ThenInclude(t => t.WorkingCosts);// récuperation pour setter la proprieté Cost;

    }
}
