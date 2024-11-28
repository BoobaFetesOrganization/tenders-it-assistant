using GenAIChat.Application.Adapter.Database.Repository;

namespace GenAIChat.Application.Adapter.Database
{
    public interface IGenAiUnitOfWorkAdapter : IDisposable
    {
        IProjectRepositoryAdapter Project { get; }
        IUserStoryGroupRepositoryAdapter UserStoryGroup { get; }
        IUserStoryRepositoryAdapter UserStory { get; }
        ITaskRepositoryAdapter Task { get; }
        ITaskCostRepositoryAdapter TaskCost { get; }
        IDocumentRepositoryAdapter Document { get; }
        IDocumentMetadataRepositoryAdapter DocumentMetadata { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
