using GenAIChat.Domain.Project;
using GenAIChat.Infrastructure.Database.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.Repository.Project
{
    public class UserStoryRepository : GenericRepository<UserStoryDomain>
    {
        public UserStoryRepository(GenAiDbContext dbContext) : base(dbContext)
        {
        }
    }
}
