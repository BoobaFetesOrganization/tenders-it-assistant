using Azure.Data.Tables;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Filter;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Common;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class DocumentRepository(TableServiceClient service) : BaseRepository<DocumentDomain>(service, "Documents")
    {
        public override Task<DocumentDomain> AddAsync(DocumentDomain domain)
        {
            throw new NotImplementedException();
        }

        public override Task<int> CountAsync(IFilter? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<bool?> DeleteAsync(DocumentDomain domain)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<DocumentDomain>> GetAllAsync(IFilter? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<Paged<DocumentDomain>> GetAllPagedAsync(PaginationOptions options, IFilter? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<DocumentDomain?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public override Task<bool?> UpdateAsync(DocumentDomain domain)
        {
            throw new NotImplementedException();
        }
    }
}
