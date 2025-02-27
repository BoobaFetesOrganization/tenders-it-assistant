using AutoMapper;
using GenAIChat.Domain.Document;
using GenAIChat.Infrastructure.Database.Sqlite.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace GenAIChat.Infrastructure.Database.Sqlite.Repository
{
    public class DocumentRepository(GenAiDbContext dbContext, IMapper mapper) : GenericRepository<DocumentDomain>(dbContext, mapper)
    {
        protected override IQueryable<DocumentDomain> GetPropertiesForCollection(IQueryable<DocumentDomain> query) => query.Include(i => i.Metadata);
        protected override IQueryable<DocumentDomain> GetProperties(IQueryable<DocumentDomain> query) => query.Include(i => i.Metadata);
    }
}
