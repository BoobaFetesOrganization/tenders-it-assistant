using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;

namespace GenAIChat.Application.Adapter.Database
{
    public interface IRepositoryAdapter<TDomain> : IDisposable where TDomain : class, IEntityDomain
    {
        Task<int> CountAsync(IFilter? filter = null);
        Task<IEnumerable<TDomain>> GetAllAsync(IFilter? filter = null);
        Task<Paged<TDomain>> GetAllPagedAsync(PaginationOptions options, IFilter? filter = null);
        Task<TDomain?> GetByIdAsync(string id);
        Task<TDomain> AddAsync(TDomain domain);
        Task<TDomain> UpdateAsync(TDomain domain);
        Task<TDomain> DeleteAsync(TDomain domain);
        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}

