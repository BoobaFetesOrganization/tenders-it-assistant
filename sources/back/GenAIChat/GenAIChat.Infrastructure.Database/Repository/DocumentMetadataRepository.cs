using GenAIChat.Application.Adapter.Database.Repository;
using GenAIChat.Domain.Document;
using GenAIChat.Infrastructure.Database.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.Repository
{
    public class DocumentMetadataRepository : GenericRepository<DocumentMetadataDomain>, IDocumentMetadataRepositoryAdapter
    {
        public DocumentMetadataRepository(GenAiDbContext dbContext) : base(dbContext)
        {
        }
    }
}
