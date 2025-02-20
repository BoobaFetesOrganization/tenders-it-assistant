using GenAIChat.Application.Adapter.Database;
using GenAIChat.Application.Resources;
using GenAIChat.Application.Usecase;
using Microsoft.Extensions.DependencyInjection;

namespace GenAIChat.Application
{
    public static class ConfigureService
    {
        public static void AddGenAiChatApplication(this IServiceCollection services, Action<string>? writeLine = null)
        {
            writeLine?.Invoke("Add Application services");

            // resource registration
            services.AddSingleton<EmbeddedResource>();

            // application registration
            services.AddScoped<ProjectApplication>();
            services.AddScoped<DocumentApplication>();
            services.AddScoped<UserStoryGroupApplication>();
            services.AddScoped<UserStoryApplication>();

            // register MediatR to scan all assemblies in the current domain
            services.AddMediatR(cfg =>
            {
                cfg.RegisterGenericHandlers = true;
                cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });
        }
    }
}
