using GenAIChat.Application.Adapter.Database.Repository;
using GenAIChat.Domain.Project.Group.UserStory.Task;
using GenAIChat.Infrastructure.Database.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace GenAIChat.Infrastructure.Database.Repository
{
    public class TaskRepository(GenAiDbContext dbContext) : GenericRepository<TaskDomain>(dbContext), ITaskRepositoryAdapter
    {
        protected override IQueryable<TaskDomain> GetProperties(IQueryable<TaskDomain> query) => query.Include(i => i.WorkingCosts);
    }
}
