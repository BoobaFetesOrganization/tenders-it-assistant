using GenAIChat.Application.Adapter.Database.Repository;

namespace GenAIChat.Application.Adapter.Database
{
    public interface IGenAiUnitOfWorkAdapter : IDisposable
    {
        IProjectRepositoryAdapter Projects { get; }
        IUserStoryRepositoryAdapter UserStories { get; }
        IUserStoryTaskRepositoryAdapter UserStoryTasks { get; }
        IDocumentRepositoryAdapter Documents { get; }
        IDocumentMetadataRepositoryAdapter DocumentMetadatas { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
