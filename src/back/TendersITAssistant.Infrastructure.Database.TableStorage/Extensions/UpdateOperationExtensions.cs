using TendersITAssistant.Domain.Common;
using TendersITAssistant.Infrastructure.Database.TableStorage.Entity.Common;
using TendersITAssistant.Infrastructure.Database.TableStorage.Repository.Generic;

namespace TendersITAssistant.Infrastructure.Database.TableStorage.Extensions
{
    internal static class UpdateOperationExtensions
    {
        public static IEnumerable<Task<bool>> ResolveOperationsWith<TDomain, TEntity>(
             this IEnumerable<TDomain> domains,
             IEnumerable<TDomain> entities,
             IGenericRepository<TDomain, TEntity> repository,
                Action<TDomain> onChange,
             CancellationToken cancellationToken)
             where TDomain : class, IEntityDomain, new()
             where TEntity : BaseEntity, new()
        {
            // arrange : select entities to delete, update and add
            var idsToDelete = entities.Select(e => e.Id).Except(domains.Select(d => d.Id));
            var idsToUpdate = entities.Select(e => e.Id).Intersect(domains.Select(d => d.Id));
            var idsToAdd = domains.Select(d => d.Id).Except(entities.Select(e => e.Id));

            // apply changes to enforce consistency
            domains.ToList().ForEach(onChange);

            // create actions in function of the operations to perform
            List<Task<bool>> actions = [];
            actions.AddRange(idsToDelete.Select(id => repository.DeleteAsync(entities.Single(d => d.Id == id), cancellationToken)));
            actions.AddRange(idsToAdd.Select(id => repository.AddAsync(domains.Single(d => d.Id == id), cancellationToken).ContinueWith(task => task.Result is not null, cancellationToken)));
            actions.AddRange(idsToUpdate.Select(id => repository.UpdateAsync(domains.Single(d => d.Id == id), cancellationToken)));

            return actions;
        }
    }
}
