using AutoMapper;
using GenAIChat.Domain.Project.Group.UserStory.Task;
using GenAIChat.Infrastructure.Database.Sqlite.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace GenAIChat.Infrastructure.Database.Sqlite.Repository
{
    public class TaskRepository(GenAiDbContext dbContext, IMapper mapper) : GenericRepository<TaskDomain>(dbContext, mapper)
    {
        protected override IQueryable<TaskDomain> GetProperties(IQueryable<TaskDomain> query) => query.Include(i => i.WorkingCosts);
    }
}
