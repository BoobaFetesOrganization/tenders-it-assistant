using Azure.Data.Tables;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Common;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class UserStoryGroupRepository(TableServiceClient service) : BaseRepository<UserStoryGroupDomain>(service, "UserStoryGroups")
    {
        public override Task<UserStoryGroupDomain> AddAsync(UserStoryGroupDomain domain)
        {
            throw new NotImplementedException();
        }

        public override Task<int> CountAsync(IFilter? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<UserStoryGroupDomain> DeleteAsync(UserStoryGroupDomain domain)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<UserStoryGroupDomain>> GetAllAsync(IFilter? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<Paged<UserStoryGroupDomain>> GetAllPagedAsync(PaginationOptions options, IFilter? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<UserStoryGroupDomain?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public override Task<UserStoryGroupDomain> UpdateAsync(UserStoryGroupDomain domain)
        {
            throw new NotImplementedException();
        }
    }
}