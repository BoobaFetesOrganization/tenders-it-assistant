using GenAIChat.Application.Adapter.File;
using Microsoft.Extensions.DependencyInjection;

namespace GenAIChat.Infrastructure
{
    public static class ConfigureService
    {
        public static void AddGenAiChatInfrastructure(this IServiceCollection services, Action<string>? writeLine = null)
        {
            writeLine?.Invoke("configure Infrastructure : FileSystem services");

            // services registration
            services.AddScoped<IFileSystemAdapter, FileSystemAdapter>();
        }
    }
}
