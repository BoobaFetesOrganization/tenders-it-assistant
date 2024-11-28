using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Adapter.Database.Repository;
using GenAIChat.Infrastructure.Database.Repository;

namespace GenAIChat.Infrastructure.Database.UnitOfWork
{
    public class UnitOfWork(GenAiDbContext dbContext) : IGenAiUnitOfWorkAdapter
    {
        public IProjectRepositoryAdapter Project { get; } = new ProjectRepository(dbContext);
        public IDocumentRepositoryAdapter Document { get; } = new DocumentRepository(dbContext);
        public IDocumentMetadataRepositoryAdapter DocumentMetadata { get; } = new DocumentMetadataRepository(dbContext);
        public IUserStoryGroupRepositoryAdapter UserStoryGroup { get; } = new UserStoryGroupRepository(dbContext);
        public IUserStoryRepositoryAdapter UserStory { get; } = new UserStoryRepository(dbContext);
        public ITaskRepositoryAdapter Task { get; } = new TaskRepository(dbContext);
        public ITaskCostRepositoryAdapter TaskCost { get; } = new TaskCostRepository(dbContext);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.SaveChangesAsync(cancellationToken);
        }

        #region implements IDisposable
        private bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // Libérer les ressources managées
                dbContext.Dispose();
            }

            // Libérer les ressources non managées si nécessaire
            _disposed = true;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
        #endregion
    }
}
