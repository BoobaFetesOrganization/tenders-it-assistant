using AutoMapper;
using Azure.Data.Tables;
using GenAIChat.Application.Specifications;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using GenAIChat.Domain.Project;
using GenAIChat.Infrastructure.Database.TableStorage.Entity;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Common;
using System.Linq.Expressions;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class ProjectRepository(TableServiceClient service, IMapper mapper) : BaseRepository<ProjectDomain>(service, "Projects")
    {
        public override Task<ProjectDomain> AddAsync(ProjectDomain entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> CountAsync(Expression<Func<ProjectDomain, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<ProjectDomain> DeleteAsync(ProjectDomain entity)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<ProjectDomain>> GetAllAsync(Expression<Func<ProjectDomain, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<ProjectDomain>> GetAllAsync2(IFilter? filter = null)
        {
            var filterString = filter is null ? null : filter.ToAzureFilterString();
            var q = client.Query<ProjectEntity>(filterString);
            throw new NotImplementedException();
        }

        public override Task<Paged<ProjectDomain>> GetAllPagedAsync(PaginationOptions options, Expression<Func<ProjectDomain, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<ProjectDomain?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public override Task<ProjectDomain> UpdateAsync(ProjectDomain entity)
        {
            throw new NotImplementedException();
        }
    }
}
