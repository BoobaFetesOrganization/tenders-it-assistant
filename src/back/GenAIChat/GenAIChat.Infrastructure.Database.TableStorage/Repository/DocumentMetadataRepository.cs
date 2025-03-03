using Azure.Data.Tables;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Filter;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Common;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class DocumentMetadataRepository(TableServiceClient service) : BaseRepository<DocumentMetadataDomain>(service, "DocumentMetadatas")
    {
        public override Task<DocumentMetadataDomain> AddAsync(DocumentMetadataDomain domain,CancellationToken cancellationToken=default)
        {
            throw new NotImplementedException();
        }

        public override Task<int> CountAsync(IFilter? filter = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<bool?> DeleteAsync(DocumentMetadataDomain domain, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<DocumentMetadataDomain>> GetAllAsync(IFilter? filter = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<Paged<DocumentMetadataDomain>> GetAllPagedAsync(PaginationOptions options, IFilter? filter = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<DocumentMetadataDomain?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<bool?> UpdateAsync(DocumentMetadataDomain domain, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
