using GenAIChat.Application.Adapter.Database.Repository;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Infrastructure.Database.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GenAIChat.Infrastructure.Database.Repository
{
    public class DocumentRepository : GenericRepository<DocumentDomain>, IDocumentRepositoryAdapter
    {
        public DocumentRepository(GenAiDbContext dbContext) : base(dbContext)
        {
        }

        protected override IQueryable<DocumentDomain> GetProperties(IQueryable<DocumentDomain> query) => query.Include(i => i.Metadata);
    }
}
