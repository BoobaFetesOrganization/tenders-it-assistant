using GenAIChat.Domain.Document;
using GenAIChat.Infrastructure.Database.Sqlite.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace GenAIChat.Infrastructure.Database.Sqlite.Repository
{
    public class DocumentRepository(GenAiDbContext dbContext) : GenericRepository<DocumentDomain>(dbContext)
    {
        protected override IQueryable<DocumentDomain> GetPropertiesForCollection(IQueryable<DocumentDomain> query) => query.Include(i => i.Metadata);
        protected override IQueryable<DocumentDomain> GetProperties(IQueryable<DocumentDomain> query) => query.Include(i => i.Metadata);
    }
}
