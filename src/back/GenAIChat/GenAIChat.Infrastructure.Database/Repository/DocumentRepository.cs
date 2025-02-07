using GenAIChat.Application.Adapter.Database.Repository;
using GenAIChat.Domain.Document;
using GenAIChat.Infrastructure.Database.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace GenAIChat.Infrastructure.Database.Repository
{
    public class DocumentRepository(GenAiDbContext dbContext) : GenericRepository<DocumentDomain>(dbContext), IDocumentRepositoryAdapter
    {
        protected override IQueryable<DocumentDomain> GetPropertiesForCollection(IQueryable<DocumentDomain> query) => query.Include(i => i.Metadata);
        protected override IQueryable<DocumentDomain> GetProperties(IQueryable<DocumentDomain> query) => query.Include(i => i.Metadata);
    }
}
