using GenAIChat.Domain.Document;
using GenAIChat.Infrastructure.Database.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.Repository.Document
{
    public class DocumentMetadataRepository : GenericRepository<DocumentMetadataDomain>
    {
        public DocumentMetadataRepository(GenAiDbContext dbContext) : base(dbContext)
        {
        }
    }
}
