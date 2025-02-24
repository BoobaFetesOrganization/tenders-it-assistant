using GenAIChat.Domain.Project.Group;
using GenAIChat.Infrastructure.Database.Sqlite.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace GenAIChat.Infrastructure.Database.Sqlite.Repository
{
    public class UserStoryPromptRepository(GenAiDbContext dbContext) : GenericRepository<UserStoryRequestDomain>(dbContext)
    {
    }
}
