using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Adapter.Database.Repository;
using GenAIChat.Infrastructure.Database.Sqlite.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GenAIChat.Infrastructure.Database.Sqlite
{
    public static class ConfigureService
    {
        private const string AssemblyNameInCharggeOfMigration = "GenAIChat.Infrastructure.Database.Sqlite.Migrations";
        
            public static void AddGenAiChatInfrastructureDatabaseSqlLite(this IServiceCollection services, IConfiguration configuration)
        {
            // database configuration
            services.AddDbContext<GenAiDbContext>(options => options.UseSqlite(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(AssemblyNameInCharggeOfMigration)
                ));
            services.AddScoped<IGenAiUnitOfWorkAdapter, UnitOfWork.UnitOfWork>();
            services.AddScoped<IProjectRepositoryAdapter, ProjectRepository>();
            services.AddScoped<IDocumentRepositoryAdapter, DocumentRepository>();
            services.AddScoped<IDocumentMetadataRepositoryAdapter, DocumentMetadataRepository>();
            services.AddScoped<IUserStoryGroupRepositoryAdapter, UserStoryGroupRepository>();
            services.AddScoped<IUserStoryRepositoryAdapter, UserStoryRepository>();
            services.AddScoped<ITaskRepositoryAdapter, TaskRepository>();
            services.AddScoped<ITaskCostRepositoryAdapter, TaskCostRepository>();
        }
    }
}
