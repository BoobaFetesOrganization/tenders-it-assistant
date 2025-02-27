using Azure.Data.Tables;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Common;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class UserStoryRequestRepository(TableServiceClient service) : BaseRepository<UserStoryRequestDomain>(service, "UserStoryRequests")
    {
        public override Task<UserStoryRequestDomain> AddAsync(UserStoryRequestDomain domain)
        {
            throw new NotImplementedException();
        }

        public override Task<int> CountAsync(IFilter? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<UserStoryRequestDomain> DeleteAsync(UserStoryRequestDomain domain)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<UserStoryRequestDomain>> GetAllAsync(IFilter? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<Paged<UserStoryRequestDomain>> GetAllPagedAsync(PaginationOptions options, IFilter? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<UserStoryRequestDomain?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public override Task<UserStoryRequestDomain> UpdateAsync(UserStoryRequestDomain domain)
        {
            throw new NotImplementedException();
        }
    }
}