using GenAIChat.Domain.Project;
using GenAIChat.Infrastructure.Database.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.Repository.Project
{
    public class UserStoryTaskRepository : GenericRepository<UserStoryTaskDomain>
    {
        public UserStoryTaskRepository(GenAiDbContext dbContext) : base(dbContext)
        {
        }
    }
}
