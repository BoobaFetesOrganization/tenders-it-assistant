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
        public const string SpaCors = "AllowSpecificOrigin";
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
            var corsConfigs = configuration.GetSection("Cors").Get<List<CorsConfiguration>>()
                ?? throw new InvalidOperationException("Cors section is missing or invalid in appsettings.json, it should be {\r\n  \"Cors\": [\r\n    {\r\n      \"Name\": \"SpaCors\",\r\n      \"Origins\": [ \"https://localhost:3000\" ],\r\n      \"AllowedVerbs\": [ \"GET\", \"POST\", \"PUT\", \"DELETE\" ],\r\n      \"AllowedHeaders\": [ \"*\" ]\r\n    }\r\n  ]\r\n}");

            Console.WriteLine(JsonSerializer.Serialize(corsConfigs, new JsonSerializerOptions { WriteIndented = true }));

            services.AddCors(options =>
            {
                corsConfigs.ForEach(corsConfig =>
                {
                    options.AddPolicy(corsConfig.Name, builder =>
                    {
                        if (corsConfig.Origins is not null) builder.WithOrigins(corsConfig.Origins.ToArray());
                        if (corsConfig.AllowedVerbs is not null) builder.WithMethods(corsConfig.AllowedVerbs.ToArray());
                        if (corsConfig.AllowedHeaders is not null) builder.WithHeaders(corsConfig.AllowedHeaders.ToArray());
                    });
                });
            });
        }

        public static void UseGenAiChatPresentationServices(this IApplicationBuilder app, IConfiguration configuration)
        {
            var corsConfigs = configuration.GetSection("Cors").Get<List<CorsConfiguration>>()
                ?? throw new InvalidOperationException("Cors section is missing or invalid in appsettings.json, it should be {\r\n  \"Cors\": [\r\n    {\r\n      \"Name\": \"SpaCors\",\r\n      \"Origins\": [ \"https://localhost:3000\" ],\r\n      \"AllowedVerbs\": [ \"GET\", \"POST\", \"PUT\", \"DELETE\" ],\r\n      \"AllowedHeaders\": [ \"*\" ]\r\n    }\r\n  ]\r\n}");


            // Utiliser la configuration CORS
            corsConfigs.ForEach(corsConfig => app.UseCors(corsConfig.Name));
        }
    }
}
