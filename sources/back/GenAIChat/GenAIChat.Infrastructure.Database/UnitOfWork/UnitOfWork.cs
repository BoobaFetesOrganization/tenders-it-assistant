using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Adapter.Database.Repository;
using GenAIChat.Infrastructure.Database.Repository;

namespace GenAIChat.Infrastructure.Database.UnitOfWork
{
    public class UnitOfWork(GenAiDbContext dbContext) : IGenAiUnitOfWorkAdapter
    {
        public IProjectRepositoryAdapter Projects { get; } = new ProjectRepository(dbContext);
        public IUserStoryRepositoryAdapter UserStories { get; } = new UserStoryRepository(dbContext);
        public IUserStoryTaskRepositoryAdapter UserStoryTasks { get; } = new UserStoryTaskRepository(dbContext);
        public IDocumentRepositoryAdapter Documents { get; } = new DocumentRepository(dbContext);
        public IDocumentMetadataRepositoryAdapter DocumentMetadatas { get; } = new DocumentMetadataRepository(dbContext);

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
