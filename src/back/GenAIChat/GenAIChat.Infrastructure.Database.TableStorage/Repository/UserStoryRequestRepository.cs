using Azure.Data.Tables;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Common;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class UserStoryRequestRepository(TableServiceClient service) : BaseRepository<UserStoryRequestDomain>(service, "UserStoryRequests")
    {
        public override Task<UserStoryRequestDomain> AddAsync(UserStoryRequestDomain domain, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<int> CountAsync(IFilter? filter = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<bool?> DeleteAsync(UserStoryRequestDomain domain, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<UserStoryRequestDomain>> GetAllAsync(IFilter? filter = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<Paged<UserStoryRequestDomain>> GetAllPagedAsync(PaginationOptions options, IFilter? filter = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<UserStoryRequestDomain?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<bool?> UpdateAsync(UserStoryRequestDomain domain, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}