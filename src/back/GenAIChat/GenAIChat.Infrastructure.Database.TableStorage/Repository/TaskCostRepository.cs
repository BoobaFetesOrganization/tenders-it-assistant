using Azure.Data.Tables;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project.Group.UserStory.Task.Cost;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Common;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class TaskCostRepository(TableServiceClient service) : BaseRepository<TaskCostDomain>(service, "TaskCosts")
    {
        public override Task<TaskCostDomain> AddAsync(TaskCostDomain domain)
        {
            throw new NotImplementedException();
        }

        public override Task<int> CountAsync(IFilter? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<bool?> DeleteAsync(TaskCostDomain domain)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<TaskCostDomain>> GetAllAsync(IFilter? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<Paged<TaskCostDomain>> GetAllPagedAsync(PaginationOptions options, IFilter? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<TaskCostDomain?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public override Task<bool?> UpdateAsync(TaskCostDomain domain)
        {
            throw new NotImplementedException();
        }
    }
}
