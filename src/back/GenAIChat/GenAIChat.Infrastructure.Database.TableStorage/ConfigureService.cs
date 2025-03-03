using Azure.Data.Tables;
using GenAIChat.Application.Adapter.Database;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project;
using GenAIChat.Domain.Project.Group;
using GenAIChat.Domain.Project.Group.UserStory;
using GenAIChat.Domain.Project.Group.UserStory.Task;
using GenAIChat.Domain.Project.Group.UserStory.Task.Cost;
using GenAIChat.Infrastructure.Database.TableStorage.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GenAIChat.Infrastructure.Database
{
    public static class ConfigureService
    {
        public static void AddGenAiChatInfrastructureDatabase(this IServiceCollection services, IConfiguration configuration, Action<string>? writeLine = null)
        {
            writeLine?.Invoke("configure Infrastructure : database : TableStorage services");

            // register AutoMapper to scan all assemblies in the current domain
            services.AddAutoMapper(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));

            services.AddDatabase(configuration, writeLine);
            services.AddRepositories();
        }

        private static void AddDatabase(this IServiceCollection services, IConfiguration configuration, Action<string>? writeLine)
        {
            var databaseProvider = configuration.GetValue<string>("DatabaseProvider") ?? throw new InvalidOperationException("The DatabaseProvider property is not set in the appsettings.json");
            writeLine?.Invoke($"use connection string named '{databaseProvider}'");
            var connectionString = configuration.GetConnectionString(databaseProvider) ?? throw new InvalidOperationException("The DatabaseProvider property's value is not found in the ConnectionStrings section");
            if (string.IsNullOrEmpty(connectionString)) throw new InvalidOperationException("The connection string cannot be null or empty.");

            var service = new TableServiceClient(connectionString);

            services.AddSingleton(service);
        }

        private static void AddRepositories(this IServiceCollection services)
        {
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
