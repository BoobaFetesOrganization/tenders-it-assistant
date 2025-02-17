using GenAIChat.Application.Resources;
using GenAIChat.Application.Usecase;
using GenAIChat.Domain.Project;
using Microsoft.Extensions.DependencyInjection;

namespace GenAIChat.Application
{
    public static class ConfigureService
    {
        public static void AddGenAiChatApplication(this IServiceCollection services)
        {
            // resource registration
            services.AddSingleton<EmbeddedResource>();

            // application registration
            services.AddScoped<ProjectApplication>();
            services.AddScoped<DocumentApplication>();
            services.AddScoped<UserStoryGroupApplication>();
            services.AddScoped<UserStoryApplication>();
            services.AddScoped<PromptApplication>();

            // register MediatR to scan all assemblies in the current domain
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        }
    }
}
