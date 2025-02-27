using AutoMapper;
using GenAIChat.Domain.Project.Group.UserStory;
using GenAIChat.Infrastructure.Database.Sqlite.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace GenAIChat.Infrastructure.Database.Sqlite.Repository
{
    public class UserStoryRepository(GenAiDbContext dbContext, IMapper mapper) : GenericRepository<UserStoryDomain>(dbContext, mapper)
    {
        protected override IQueryable<UserStoryDomain> GetPropertiesForCollection(IQueryable<UserStoryDomain> query) => query.Include(i => i.Tasks);
        protected override IQueryable<UserStoryDomain> GetProperties(IQueryable<UserStoryDomain> query) => query
            .Include(us => us.Tasks)
                .ThenInclude(t => t.WorkingCosts);

    }
}
