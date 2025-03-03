using AutoMapper;
using Azure.Data.Tables;
using GenAIChat.Domain.Document;
using GenAIChat.Infrastructure.Database.TableStorage.Entity;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    internal class DocumentMetadataRepository(TableServiceClient service, IMapper mapper) : GenericRepository<DocumentMetadataDomain, DocumentMetadataEntity>(service, "DocumentMetadatas", mapper)
    {
    }
}
