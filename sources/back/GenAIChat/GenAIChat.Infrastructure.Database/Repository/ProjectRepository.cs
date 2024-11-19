using GenAIChat.Application.Adapter.Database.Repository;
using GenAIChat.Domain.Project;
using GenAIChat.Infrastructure.Database.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace GenAIChat.Infrastructure.Database.Repository
{
    public class ProjectRepository : GenericRepository<ProjectDomain>, IProjectRepositoryAdapter
    {
        public ProjectRepository(GenAiDbContext dbContext) : base(dbContext)
        {
        }
        protected override IQueryable<ProjectDomain> GetProperties(IQueryable<ProjectDomain> query) => query
            .Include(i => i.PromptResponse)
            .Include(i => i.Documents)
            .Include(i => i.UserStories)
                .ThenInclude(us => us.Tasks);
    }
}
