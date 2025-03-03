using Azure.Data.Tables;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project.Group.UserStory.Task;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Common;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class TaskRepository(TableServiceClient service) : BaseRepository<TaskDomain>(service, "Tasks")
    {
        public override Task<TaskDomain> AddAsync(TaskDomain domain, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<int> CountAsync(IFilter? filter = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<bool?> DeleteAsync(TaskDomain domain, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<TaskDomain>> GetAllAsync(IFilter? filter = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<Paged<TaskDomain>> GetAllPagedAsync(PaginationOptions options, IFilter? filter = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<TaskDomain?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<bool?> UpdateAsync(TaskDomain domain, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}