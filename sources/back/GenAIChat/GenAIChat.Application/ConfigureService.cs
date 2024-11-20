using GenAIChat.Application.Usecase;
using Microsoft.Extensions.DependencyInjection;

namespace GenAIChat.Application
{
    public static class ConfigureService
    {
        public static void AddGenAiChatApplicationServices(this IServiceCollection services)
        {
            // application registration
            services.AddScoped<ProjectApplication>();
            services.AddScoped<DocumentApplication>();
            services.AddScoped<PromptApplication>();

            // register MediatR to scan all assemblies in the current domain
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        }
    }
}
