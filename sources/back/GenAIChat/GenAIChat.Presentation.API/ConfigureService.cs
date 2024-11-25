using GenAIChat.Application;
using GenAIChat.Infrastructure;
using GenAIChat.Infrastructure.Api.Gemini;
using GenAIChat.Infrastructure.Api.Gemini.Service;
using GenAIChat.Infrastructure.Database;
using GenAIChat.Presentation.API.Configuation;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;

namespace GenAIChat.Presentation.API
{
    public static class ConfigureService
    {
        public const string SpaCors = "SpaCors";
        public static void AddGenAiChatServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddGenAiChatPresentationServices(configuration);

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

        private static void AddGenAiChatPresentationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // register AutoMapper to scan all assemblies in the current domain
            services.AddAutoMapper(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));

            // register the configuration for the CORS policy
            Console.WriteLine("Persenter.API : Conviguration : Cors:");
            CorsConfiguration corsConfig = configuration.GetSection("Cors:SpaCors").Get<CorsConfiguration>()
                ?? throw new InvalidOperationException("Cors section is missing or invalid in appsettings.json, it should be {\r\n  \"Cors\": {\r\n    \"SpaCors\": {\r\n      \"Name\": \"SpaCors\",\r\n      \"Origins\": [ \"http://localhost:3000\", \"https://localhost:3000\" ],\r\n      \"AllowedVerbs\": [ \"GET\", \"POST\", \"PUT\", \"DELETE\", \"OPTIONS\" ],\r\n      \"AllowedHeaders\": [ \"*\" ]\r\n    }\r\n  }\r\n}");

            Console.WriteLine(JsonSerializer.Serialize(corsConfig, new JsonSerializerOptions { WriteIndented = true }));

            services.AddCors(options =>
            {
                options.AddPolicy(SpaCors, builder =>
                {
                    if (corsConfig.Origins is not null) builder.WithOrigins(corsConfig.Origins.ToArray());
                    if (corsConfig.AllowedVerbs is not null) builder.WithMethods(corsConfig.AllowedVerbs.ToArray());
                    if (corsConfig.AllowedHeaders is not null) builder.WithHeaders(corsConfig.AllowedHeaders.ToArray());
                });
            });
        }

        public static void UseGenAiChatPresentationServices(this WebApplication app, IConfiguration configuration)
        {
            var corsConfig = configuration.GetSection("Cors:SpaCors").Get<CorsConfiguration>()
                ?? throw new InvalidOperationException("Cors section is missing or invalid in appsettings.json, it should be {\r\n  \"Cors\": {\r\n    \"SpaCors\": {\r\n      \"Name\": \"SpaCors\",\r\n      \"Origins\": [ \"http://localhost:3000\", \"https://localhost:3000\" ],\r\n      \"AllowedVerbs\": [ \"GET\", \"POST\", \"PUT\", \"DELETE\", \"OPTIONS\" ],\r\n      \"AllowedHeaders\": [ \"*\" ]\r\n    }\r\n  }\r\n}");

            app.UseRouting();
            app.UseCors(SpaCors);
            app.UseAuthorization();
            app.MapControllers();
            
            // Serve the SPA for the root URL
            app.MapFallbackToFile("/index.html");

            // Configurer le middleware pour servir la SPA
            app.UseDefaultFiles();
            app.UseStaticFiles();

        }
    }
}
