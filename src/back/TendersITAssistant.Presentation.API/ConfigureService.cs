using System.Text.Json;
using TendersITAssistant.Presentation.API.Configuation;
using ILogger = Serilog.ILogger;

namespace TendersITAssistant.Presentation.API
{
    public static class ConfigureService
    {
        public const string SpaCors = "SpaCors";

        private static readonly JsonSerializerOptions JsonSerializerOptions = new() { WriteIndented = true };

        public static void AddPresentationApi(this IServiceCollection services, IConfiguration configuration, ILogger logger)
        {
            logger.Information("configure Presentation : Web Api services");

            // register AutoMapper to scan all assemblies in the current domain
            services.AddAutoMapper(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));

            // register the configuration for the CORS policy
            logger.Information("Persenter.API : Configuration : Cors:");
            CorsConfiguration corsConfig = configuration.GetSection("Cors:SpaCors").Get<CorsConfiguration>()
                ?? throw new InvalidOperationException("Cors section is missing or invalid in appsettings.json, it should be {\r\n  \"Cors\": {\r\n    \"SpaCors\": {\r\n      \"Name\": \"SpaCors\",\r\n      \"Origins\": [ \"http://localhost:3000\", \"https://localhost:3000\" ],\r\n      \"AllowedVerbs\": [ \"GET\", \"POST\", \"PUT\", \"DELETE\", \"OPTIONS\" ],\r\n      \"AllowedHeaders\": [ \"*\" ]\r\n    }\r\n  }\r\n}");

            Console.WriteLine(JsonSerializer.Serialize(corsConfig, JsonSerializerOptions));

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

        public static void UsePresentationApi(this IApplicationBuilder builder)
        {
            builder.UseCors(SpaCors);
        }
    }
}
