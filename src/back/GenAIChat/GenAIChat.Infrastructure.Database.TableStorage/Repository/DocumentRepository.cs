using Azure.Data.Tables;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Filter;
using GenAIChat.Infrastructure.Database.TableStorage.Repository.Common;
using System.Linq.Expressions;

namespace GenAIChat.Infrastructure.Database.TableStorage.Repository
{
    public class DocumentRepository(TableServiceClient service) : BaseRepository<DocumentDomain>(service, "Documents")
    {
        public override Task<DocumentDomain> AddAsync(DocumentDomain entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> CountAsync(Expression<Func<DocumentDomain, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<DocumentDomain> DeleteAsync(DocumentDomain entity)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<DocumentDomain>> GetAllAsync(Expression<Func<DocumentDomain, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<DocumentDomain>> GetAllAsync2(IFilter? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<Paged<DocumentDomain>> GetAllPagedAsync(PaginationOptions options, Expression<Func<DocumentDomain, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public override Task<DocumentDomain?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public override Task<DocumentDomain> UpdateAsync(DocumentDomain entity)
        {
            throw new NotImplementedException();
        }
    }
}
