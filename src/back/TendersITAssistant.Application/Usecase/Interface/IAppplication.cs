using TendersITAssistant.Domain.Common;
using TendersITAssistant.Domain.Filter;

namespace TendersITAssistant.Application.Usecase.Interface
{
    public interface IApplication<TDomain> where TDomain : EntityDomain, new()
    {
        public Task<Paged<TDomain>> GetAllPagedAsync(PaginationOptions options, IFilter? filter = null, CancellationToken cancellationToken = default);

        public Task<TDomain?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

        public Task<TDomain> CreateAsync(TDomain domain, CancellationToken cancellationToken = default);

        public Task<bool> UpdateAsync(TDomain domain, CancellationToken cancellationToken = default);

        public Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}
