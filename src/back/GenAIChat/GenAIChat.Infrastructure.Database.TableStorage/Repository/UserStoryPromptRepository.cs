using Azure.Data.Tables;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Common;
using System.Linq.Expressions;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class UserStoryPromptRepository(TableServiceClient service) : BaseRepository<UserStoryRequestDomain>(service, "UserStoryPrompts")
    {
        public override Task<UserStoryRequestDomain> AddAsync(UserStoryRequestDomain entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> CountAsync(Expression<Func<UserStoryRequestDomain, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<UserStoryRequestDomain> DeleteAsync(UserStoryRequestDomain entity)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<UserStoryRequestDomain>> GetAllAsync(Expression<Func<UserStoryRequestDomain, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<UserStoryRequestDomain>> GetAllAsync2(IFilter? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<Paged<UserStoryRequestDomain>> GetAllPagedAsync(PaginationOptions options, Expression<Func<UserStoryRequestDomain, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<UserStoryRequestDomain?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public override Task<UserStoryRequestDomain> UpdateAsync(UserStoryRequestDomain entity)
        {
            throw new NotImplementedException();
        }
    }
}