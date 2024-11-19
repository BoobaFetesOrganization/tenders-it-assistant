using GenAIChat.Application;
using GenAIChat.Infrastructure;
using GenAIChat.Infrastructure.Api.Gemini;
using GenAIChat.Infrastructure.Api.Gemini.Service;
using GenAIChat.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GenAIChat.Presentation.API
{
    public static class ConfigureService
    {
        public static void AddGenAiChatServices(this IServiceCollection services, IConfiguration configuration)
        {
            // register AutoMapper to scan all assemblies in the current domain
            services.AddAutoMapper(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));

            // add services of the layers

            services.AddGenAiChatApplicationServices();

            services.AddGenAiChatInfrastructureServices(configuration);
            services.AddGenAiChatDatabaseServices(options => options.UseSqlite(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name)
                ));

            services.AddGenAiChatApiServices(configuration, () =>
            {
                // services configuration
                services.AddHttpClient<GeminiGenerateContentService>();
                services.AddHttpClient<GeminiFileService>();
            });
        }
    }
}
