using AutoMapper;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Infrastructure.Database.Sqlite.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.Sqlite.Repository
{
    public class UserStoryRequestRepository(GenAiDbContext dbContext, IMapper mapper) : GenericRepository<UserStoryRequestDomain>(dbContext, mapper)
    {
    }
}
