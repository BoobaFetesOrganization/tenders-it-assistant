using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Adapter.Database.Repository;
using GenAIChat.Infrastructure.Database.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GenAIChat.Infrastructure.Database
{
    public static class ConfigureService
    {
        public static void AddGenAiChatDatabaseServices(this IServiceCollection services, Action<DbContextOptionsBuilder> dbContextBuilder)
        {
            // database configuration
            services.AddDbContext<GenAiDbContext>(dbContextBuilder);
            services.AddScoped<IGenAiUnitOfWorkAdapter, UnitOfWork.UnitOfWork>();
            services.AddScoped<IProjectRepositoryAdapter, ProjectRepository>();
            services.AddScoped<IUserStoryRepositoryAdapter, UserStoryRepository>();
            services.AddScoped<IUserStoryTaskRepositoryAdapter, UserStoryTaskRepository>();
            services.AddScoped<IDocumentRepositoryAdapter, DocumentRepository>();
            services.AddScoped<IDocumentMetadataRepositoryAdapter, DocumentMetadataRepository>();
        }
    }
}
