using Azure.Data.Tables;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Common;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class UserStoryGroupRepository(TableServiceClient service) : BaseRepository<UserStoryGroupDomain>(service, "UserStoryGroups")
    {
        public override Task<UserStoryGroupDomain> AddAsync(UserStoryGroupDomain domain, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<int> CountAsync(IFilter? filter = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<bool?> DeleteAsync(UserStoryGroupDomain domain, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<UserStoryGroupDomain>> GetAllAsync(IFilter? filter = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<Paged<UserStoryGroupDomain>> GetAllPagedAsync(PaginationOptions options, IFilter? filter = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<UserStoryGroupDomain?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<bool?> UpdateAsync(UserStoryGroupDomain domain, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}