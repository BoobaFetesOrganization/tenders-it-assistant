using Azure.Data.Tables;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project.Group.UserStory.Task;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Common;
using System.Linq.Expressions;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class TaskRepository(TableServiceClient service) : BaseRepository<TaskDomain>(service, "Tasks")
    {
        public override Task<TaskDomain> AddAsync(TaskDomain domain)
        {
            throw new NotImplementedException();
        }

        public override Task<int> CountAsync(Expression<Func<TaskDomain, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<TaskDomain> DeleteAsync(TaskDomain domain)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<TaskDomain>> GetAllAsync(Expression<Func<TaskDomain, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<TaskDomain>> GetAllAsync2(IFilter? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<Paged<TaskDomain>> GetAllPagedAsync(PaginationOptions options, Expression<Func<TaskDomain, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<TaskDomain?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public override Task<TaskDomain> UpdateAsync(TaskDomain domain)
        {
            throw new NotImplementedException();
        }
    }
}