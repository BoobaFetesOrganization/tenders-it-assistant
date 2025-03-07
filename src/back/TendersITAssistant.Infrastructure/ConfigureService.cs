using TendersITAssistant.Application.Adapter.File;
using Microsoft.Extensions.DependencyInjection;

namespace TendersITAssistant.Infrastructure
{
    public static class ConfigureService
    {
        public static void AddInfrastructure(this IServiceCollection services, Action<string>? writeLine = null)
        {
            writeLine?.Invoke("configure Infrastructure : FileSystem services");

            // services registration
            services.AddScoped<IFileSystemAdapter, FileSystemAdapter>();
        }
    }
}
