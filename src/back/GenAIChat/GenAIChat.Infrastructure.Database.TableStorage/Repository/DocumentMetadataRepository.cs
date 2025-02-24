using Azure.Data.Tables;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Filter;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Common;
using System.Linq.Expressions;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class DocumentMetadataRepository(TableServiceClient service) : BaseRepository<DocumentMetadataDomain>(service, "DocumentMetadatas")
    {
        public override Task<DocumentMetadataDomain> AddAsync(DocumentMetadataDomain entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> CountAsync(Expression<Func<DocumentMetadataDomain, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<DocumentMetadataDomain> DeleteAsync(DocumentMetadataDomain entity)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<DocumentMetadataDomain>> GetAllAsync(Expression<Func<DocumentMetadataDomain, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<DocumentMetadataDomain>> GetAllAsync2(IFilter? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<Paged<DocumentMetadataDomain>> GetAllPagedAsync(PaginationOptions options, Expression<Func<DocumentMetadataDomain, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<DocumentMetadataDomain?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public override Task<DocumentMetadataDomain> UpdateAsync(DocumentMetadataDomain entity)
        {
            throw new NotImplementedException();
        }
    }
}
