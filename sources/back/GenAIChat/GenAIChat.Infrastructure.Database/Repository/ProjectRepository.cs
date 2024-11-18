using GenAIChat.Domain;
using GenAIChat.Infrastructure.Database.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.Repository
{
    public class ProjectRepository : GenericRepository<ProjectDomain>
    {
        public ProjectRepository(GenAiDbContext dbContext) : base(dbContext)
        {
        }
    }
}
