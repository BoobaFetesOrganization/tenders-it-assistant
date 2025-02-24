using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using System.Linq.Expressions;

namespace GenAIChat.Application.Adapter.Database
{
    public interface IRepositoryAdapter<TDomain> : IDisposable where TDomain : class, IEntityDomain
    {
        Task<int> CountAsync(Expression<Func<TDomain, bool>>? filter = null);
        Task<IEnumerable<TDomain>> GetAllAsync2(IFilter? filter = null);
        Task<IEnumerable<TDomain>> GetAllAsync(Expression<Func<TDomain, bool>>? filter = null);
        Task<Paged<TDomain>> GetAllPagedAsync(PaginationOptions options, Expression<Func<TDomain, bool>>? filter = null);
        Task<TDomain?> GetByIdAsync(string id);
        Task<TDomain> AddAsync(TDomain entity);
        Task<TDomain> UpdateAsync(TDomain entity);
        Task<TDomain> DeleteAsync(TDomain entity);
        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}

