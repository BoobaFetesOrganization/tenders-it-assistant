using Azure.Data.Tables;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Filter;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Common;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class DocumentRepository(TableServiceClient service) : BaseRepository<DocumentDomain>(service, "Documents")
    {
        public override Task<DocumentDomain> AddAsync(DocumentDomain domain, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<int> CountAsync(IFilter? filter = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<bool?> DeleteAsync(DocumentDomain domain, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<DocumentDomain>> GetAllAsync(IFilter? filter = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<Paged<DocumentDomain>> GetAllPagedAsync(PaginationOptions options, IFilter? filter = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<DocumentDomain?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<bool?> UpdateAsync(DocumentDomain domain, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
