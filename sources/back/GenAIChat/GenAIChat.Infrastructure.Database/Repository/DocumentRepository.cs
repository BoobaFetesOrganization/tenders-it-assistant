using GenAIChat.Domain;
using GenAIChat.Infrastructure.Database.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.Repository
{
    public class DocumentRepository : GenericRepository<DocumentDomain>
    {
        public DocumentRepository(GenAiDbContext dbContext) : base(dbContext)
        {
        }
    }
}
