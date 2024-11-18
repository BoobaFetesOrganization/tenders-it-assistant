using GenAIChat.Domain;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project;
using GenAIChat.Infrastructure.Database.Repository.Generic;

namespace GenAIChat.Application.Adapter
{
    public interface IGenAiUnitOfWorkAdapter : IDisposable
    {
        IRepositoryAdapter<ProjectDomain> Projects { get; }
        IRepositoryAdapter<UserStoryDomain> Userstories { get; }
        IRepositoryAdapter<UserStoryTaskDomain> UserStoryTasks { get; }
        IRepositoryAdapter<DocumentDomain> Documents { get; }
        IRepositoryAdapter<DocumentMetadataDomain> DocumentMetadatas { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
