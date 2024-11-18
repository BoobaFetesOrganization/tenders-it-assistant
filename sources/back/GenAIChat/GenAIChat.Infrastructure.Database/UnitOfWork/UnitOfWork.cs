using GenAIChat.Application.Adapter;
using GenAIChat.Domain;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project;
using GenAIChat.Infrastructure.Database.Repository;
using GenAIChat.Infrastructure.Database.Repository.Document;
using GenAIChat.Infrastructure.Database.Repository.Generic;
using GenAIChat.Infrastructure.Database.Repository.Project;
using Microsoft.EntityFrameworkCore;

namespace GenAIChat.Infrastructure.Database.UnitOfWork
{
    public class UnitOfWork : IGenAiUnitOfWorkAdapter
    {
        private readonly ProjectRepository _projectRepository;
        private readonly UserStoryRepository _userStoryRepository;
        private readonly UserStoryTaskRepository _userStoryTaskRepository;
        private readonly DocumentRepository _documentRepository;
        private readonly DocumentMetadataRepository _documentMetadataRepository;

        public IRepositoryAdapter<ProjectDomain> Projects => _projectRepository;
        public IRepositoryAdapter<UserStoryDomain> Userstories => _userStoryRepository;
        public IRepositoryAdapter<UserStoryTaskDomain> UserStoryTasks => _userStoryTaskRepository;
        public IRepositoryAdapter<DocumentDomain> Documents => _documentRepository;
        public IRepositoryAdapter<DocumentMetadataDomain> DocumentMetadatas => _documentMetadataRepository;


        private readonly GenAiDbContext _dbContext;

        public UnitOfWork(GenAiDbContext dbContext)
        {
            _dbContext = dbContext;
            _projectRepository = new ProjectRepository(_dbContext);
            _userStoryRepository = new UserStoryRepository(_dbContext);
            _userStoryTaskRepository = new UserStoryTaskRepository(_dbContext);
            _documentRepository = new DocumentRepository(_dbContext);
            _documentMetadataRepository = new DocumentMetadataRepository(_dbContext);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
