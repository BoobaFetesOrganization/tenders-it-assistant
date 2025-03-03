using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;

namespace GenAIChat.Application.Adapter.Database
{
    public interface IRepositoryAdapter<TDomain> : IDisposable where TDomain : class, IEntityDomain
    {
        Task<int> CountAsync(IFilter? filter = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<TDomain>> GetAllAsync(IFilter? filter = null, CancellationToken cancellationToken = default);
        Task<Paged<TDomain>> GetAllPagedAsync(PaginationOptions options, IFilter? filter = null, CancellationToken cancellationToken = default);
        Task<TDomain?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<TDomain> AddAsync(TDomain domain, CancellationToken cancellationToken = default);
        Task<bool?> UpdateAsync(TDomain domain, CancellationToken cancellationToken = default);
        Task<bool?> DeleteAsync(TDomain domain, CancellationToken cancellationToken = default);
        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}

