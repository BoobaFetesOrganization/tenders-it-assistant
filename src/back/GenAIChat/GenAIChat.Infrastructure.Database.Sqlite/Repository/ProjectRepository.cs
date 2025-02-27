using AutoMapper;
using GenAIChat.Domain.Project;
using GenAIChat.Infrastructure.Database.Sqlite.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace GenAIChat.Infrastructure.Database.Sqlite.Repository
{
    public class ProjectRepository(GenAiDbContext dbContext, IMapper mapper) : GenericRepository<ProjectDomain>(dbContext, mapper)
    {
        protected override IQueryable<ProjectDomain> GetProperties(IQueryable<ProjectDomain> query) => query
            .Include(i => i.Documents)
                .ThenInclude(d => d.Metadata);
    }
}
