using AutoMapper;
using Azure.Data.Tables;
using TendersITAssistant.Domain.Document;
using TendersITAssistant.Infrastructure.Database.TableStorage.Entity;
using TendersITAssistant.Infrastructure.Database.TableStorage.Repository.Generic;

namespace TendersITAssistant.Infrastructure.Database.TableStorage.Repository
{
    internal class DocumentMetadataRepository(TableServiceClient service, IMapper mapper) : GenericRepository<DocumentMetadataDomain, DocumentMetadataEntity>(service, "DocumentMetadatas", mapper)
    {
    }
}
