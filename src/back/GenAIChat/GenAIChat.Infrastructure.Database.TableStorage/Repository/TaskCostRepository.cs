using Azure.Data.Tables;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project.Group.UserStory.Task.Cost;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Common;
using System.Linq.Expressions;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class TaskCostRepository(TableServiceClient service) : BaseRepository<TaskCostDomain>(service, "TaskCosts")
    {
        public override Task<TaskCostDomain> AddAsync(TaskCostDomain entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> CountAsync(Expression<Func<TaskCostDomain, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<TaskCostDomain> DeleteAsync(TaskCostDomain entity)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<TaskCostDomain>> GetAllAsync(Expression<Func<TaskCostDomain, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<TaskCostDomain>> GetAllAsync2(IFilter? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<Paged<TaskCostDomain>> GetAllPagedAsync(PaginationOptions options, Expression<Func<TaskCostDomain, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<TaskCostDomain?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public override Task<TaskCostDomain> UpdateAsync(TaskCostDomain entity)
        {
            throw new NotImplementedException();
        }
    }
}
