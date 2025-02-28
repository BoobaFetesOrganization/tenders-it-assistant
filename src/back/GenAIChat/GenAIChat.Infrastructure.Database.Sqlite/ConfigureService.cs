using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Domain.Project.Group.UserStory;
using GenAIChat.Domain.Project.Group.UserStory.Task;
using GenAIChat.Domain.Project.Group.UserStory.Task.Cost;
using GenAIChat.Infrastructure.Database.Sqlite;
using GenAIChat.Infrastructure.Database.Sqlite.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GenAIChat.Infrastructure.Database
{
    public static class ConfigureService
    {
        private const string AssemblyNameInCharggeOfMigration = "GenAIChat.Infrastructure.Database.Sqlite.Migrations";

        public static void AddGenAiChatInfrastructureDatabase(this IServiceCollection services, IConfiguration configuration, Action<string>? writeLine = null)
        {
            writeLine?.Invoke("configure Infrastructure : database : SqlLite services");

            // register AutoMapper to scan all assemblies in the current domain
            services.AddAutoMapper(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));

            // database configuration
            var databaseProvider = configuration.GetValue<string>("DatabaseProvider") ?? throw new InvalidOperationException("The DatabaseProvider property is not set in the appsettings.json");
            writeLine?.Invoke($"use connection string named '{databaseProvider}'");
            services.AddDbContext<GenAiDbContext>(options => options.UseSqlite(
                configuration.GetConnectionString(databaseProvider),
                b => b.MigrationsAssembly(AssemblyNameInCharggeOfMigration)
                ));

            services.AddScoped<IRepositoryAdapter<ProjectDomain>, ProjectRepository>();
            services.AddScoped<IRepositoryAdapter<DocumentDomain>, DocumentRepository>();
            services.AddScoped<IRepositoryAdapter<DocumentMetadataDomain>, DocumentMetadataRepository>();
            services.AddScoped<IRepositoryAdapter<UserStoryGroupDomain>, UserStoryGroupRepository>();
            services.AddScoped<IRepositoryAdapter<UserStoryRequestDomain>, UserStoryRequestRepository>();
            services.AddScoped<IRepositoryAdapter<UserStoryDomain>, UserStoryRepository>();
            services.AddScoped<IRepositoryAdapter<TaskDomain>, TaskRepository>();
            services.AddScoped<IRepositoryAdapter<TaskCostDomain>, TaskCostRepository>();
        }
    }
}
