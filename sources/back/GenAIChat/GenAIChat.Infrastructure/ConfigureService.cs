using GenAIChat.Application.Adapter.File;
using GenAIChat.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace GenAIChat.Infrastructure
{
    public static class ConfigureService
    {
        public static void AddGenAiChatInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // app settings configuration            
            Console.WriteLine("Infrastructure : Configuration : 'AI:Prompting' :");
            var promptConfig = configuration.GetSection("AI:Prompting").Get<PromptConfiguration>()
                ?? throw new InvalidOperationException("AI:Prompting section is missing or invalid in appsettings.json, it should be { \"AI\": { \"Prompting\": { \"UserStories\": \"something\" } } }");

            Console.WriteLine(JsonSerializer.Serialize(promptConfig, new JsonSerializerOptions { WriteIndented = true }));

            services.AddSingleton(promptConfig);

            // services registration
            services.AddScoped<IFileSystemAdapter, FileSystemAdapter>();
        }
    }
}
