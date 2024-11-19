using GenAIChat.Application.Adapter.File;
using GenAIChat.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace GenAIChat.Infrastructure
{
    public static class ConfigureService
    {
        public static void AddGenAiChatInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // app settings configuration            
            var promptConfig = configuration.GetSection("AI:Prompting").Get<PromptConfiguration>()
                ?? throw new InvalidOperationException("AI:Prompting section is missing or invalid in appsettings.json, it should be { \"AI\": { \"Prompting\": { \"UserStories\": \"something\" } } }");

            services.AddSingleton(promptConfig);

            // services registration
            services.AddScoped<IFileSystemAdapter, FileSystemAdapter>();
        }
    }
}
