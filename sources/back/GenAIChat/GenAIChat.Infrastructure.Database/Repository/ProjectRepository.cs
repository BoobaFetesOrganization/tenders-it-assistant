using GenAIChat.Application.Adapter.Database.Repository;
using GenAIChat.Domain.Project;
using GenAIChat.Infrastructure.Database.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace GenAIChat.Infrastructure.Database.Repository
{
    public class ProjectRepository(GenAiDbContext dbContext) : GenericRepository<ProjectDomain>(dbContext), IProjectRepositoryAdapter
    {
        protected override IQueryable<ProjectDomain> GetProperties(IQueryable<ProjectDomain> query) => query
            .Include(i => i.Documents)
                .ThenInclude(d => d.Metadata)
            .Include(i => i.Stories)
                .ThenInclude(g => g.UserStories)
                    .ThenInclude(us => us.Tasks);
    }
}
