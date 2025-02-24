using Azure.Data.Tables;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project.Group.UserStory;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Common;
using System.Linq.Expressions;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class UserStoryRepository(TableServiceClient service) : BaseRepository<UserStoryDomain>(service, "UserStories")
    {
        public override Task<UserStoryDomain> AddAsync(UserStoryDomain entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> CountAsync(Expression<Func<UserStoryDomain, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<UserStoryDomain> DeleteAsync(UserStoryDomain entity)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<UserStoryDomain>> GetAllAsync(Expression<Func<UserStoryDomain, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<UserStoryDomain>> GetAllAsync2(IFilter? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<Paged<UserStoryDomain>> GetAllPagedAsync(PaginationOptions options, Expression<Func<UserStoryDomain, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<UserStoryDomain?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public override Task<UserStoryDomain> UpdateAsync(UserStoryDomain entity)
        {
            throw new NotImplementedException();
        }
    }
}