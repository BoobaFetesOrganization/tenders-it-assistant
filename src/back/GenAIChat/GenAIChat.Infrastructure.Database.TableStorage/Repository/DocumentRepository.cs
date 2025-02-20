using Azure.Data.Tables;
using GenAIChat.Domain.Document;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Generic;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class DocumentRepository(TableServiceClient service) : GenericRepository<DocumentDomain>(service, "Documents")
    {
    }
}
