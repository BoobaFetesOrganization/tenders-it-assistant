using GenAIChat.Application.Adapter.Database.Repository;
using GenAIChat.Domain.Project;
using GenAIChat.Infrastructure.Database.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.Repository
{
    public class UserStoryTaskRepository : GenericRepository<UserStoryTaskDomain>, IUserStoryTaskRepositoryAdapter
    {
        public UserStoryTaskRepository(GenAiDbContext dbContext) : base(dbContext)
        {
        }
    }
}
