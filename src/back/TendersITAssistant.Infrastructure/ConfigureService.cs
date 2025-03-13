using TendersITAssistant.Application.Adapter.File;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace TendersITAssistant.Infrastructure
{
    public static class ConfigureService
    {
        public static void AddInfrastructure(this IServiceCollection services, ILogger logger)
        {
            logger.Information("configure Infrastructure : FileSystem services");

            // services registration
            services.AddScoped<IFileSystemAdapter, FileSystemAdapter>();
        }
    }
}
