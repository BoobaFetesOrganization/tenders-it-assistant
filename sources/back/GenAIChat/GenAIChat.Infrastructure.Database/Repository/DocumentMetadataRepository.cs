using GenAIChat.Application.Adapter.Database.Repository;
using GenAIChat.Domain.Document;
using GenAIChat.Infrastructure.Database.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.Repository
{
    public class DocumentMetadataRepository(GenAiDbContext dbContext) : GenericRepository<DocumentMetadataDomain>(dbContext), IDocumentMetadataRepositoryAdapter
    {
    }
}
