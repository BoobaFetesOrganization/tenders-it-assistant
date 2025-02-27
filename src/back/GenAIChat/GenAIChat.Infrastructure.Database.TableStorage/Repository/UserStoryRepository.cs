using Azure.Data.Tables;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project.Group.UserStory;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Common;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class UserStoryRepository(TableServiceClient service) : BaseRepository<UserStoryDomain>(service, "UserStories")
    {
        public override Task<UserStoryDomain> AddAsync(UserStoryDomain domain)
        {
            throw new NotImplementedException();
        }

        public override Task<int> CountAsync(IFilter? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<bool?> DeleteAsync(UserStoryDomain domain)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<UserStoryDomain>> GetAllAsync(IFilter? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<Paged<UserStoryDomain>> GetAllPagedAsync(PaginationOptions options, IFilter? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<UserStoryDomain?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public override Task<bool?> UpdateAsync(UserStoryDomain domain)
        {
            throw new NotImplementedException();
        }
    }
}