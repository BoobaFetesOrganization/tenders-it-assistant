using GenAIChat.Application.Adapter.File;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GenAIChat.Infrastructure
{
    public static class ConfigureService
    {
        public static void AddGenAiChatInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // services registration
            services.AddScoped<IFileSystemAdapter, FileSystemAdapter>();
        }
    }
}
