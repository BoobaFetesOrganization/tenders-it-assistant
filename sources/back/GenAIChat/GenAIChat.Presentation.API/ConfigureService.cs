using GenAIChat.Application;
using GenAIChat.Application.Adapter;
using GenAIChat.Infrastructure;
using GenAIChat.Infrastructure.Api.Gemini;
using GenAIChat.Infrastructure.Api.Gemini.Configuation;
using GenAIChat.Infrastructure.Api.Gemini.Service;
using GenAIChat.Infrastructure.Configuration;
using GenAIChat.Infrastructure.Database;
using GenAIChat.Infrastructure.Database.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GenAIChat.Presentation.API
{
    public static class ConfigureService
    {
        public static void AddGenAiChatServices(this IServiceCollection services, IConfiguration configuration)
        {
            // app settings configuration
            var geminiApiConfig = configuration.GetSection("AI:Gemini").Get<GeminiApiConfiguration>()
              ?? throw new InvalidOperationException("AI:Gemini section is missing or invalid in appsettings.json, it should be { \"AI\": { \"Gemini\": { \"Version\": \"something\", \"ApiKey\": \"something\" } }}");
            var promptConfig = configuration.GetSection("AI:Prompting").Get<PromptConfiguration>()
                ?? throw new InvalidOperationException("AI:Prompting section is missing or invalid in appsettings.json, it should be { \"AI\": { \"Prompting\": { \"UserStories\": \"something\" } } }");

            services.AddSingleton(geminiApiConfig);
            services.AddSingleton(promptConfig);

            // database configuration
            services.AddDbContext<GenAiDbContext>(options => options.UseSqlite(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name)
                ));
            services.AddScoped<IGenAiUnitOfWorkAdapter, UnitOfWork>();

            // services configuration
            services.AddHttpClient<GeminiGenerateContentService>();
            services.AddHttpClient<GeminiFileService>();

            // services registration
            services.AddScoped<IFileSystemAdapter, FileSystemAdapter>();
            services.AddScoped<IGenAiApiAdapter, GenAiApiAdapter>();

            // application registration
            services.AddScoped<ProjectApplication>();
            services.AddScoped<PromptApplication>();

            // Presentation mapping
            services.AddAutoMapper(typeof(Program));
        }
    }
}
