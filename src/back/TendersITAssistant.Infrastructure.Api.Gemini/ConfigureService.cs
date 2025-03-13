using TendersITAssistant.Application.Adapter.Api;
using TendersITAssistant.Infrastructure.Api.Gemini.Configuation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Serilog;

namespace TendersITAssistant.Infrastructure.Api.Gemini
{
    public static class ConfigureService
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new() { WriteIndented = true };

        public static void AddInfrastructureApiGemini(this IServiceCollection services, IConfiguration configuration, Action addHttpClientCb, ILogger logger)
        {
            logger.Information("configure Infrastructure : Api : Gemini Api services");

            // app settings configuration 
            logger.Information("Infrastructure.Api.Gemini : Configuration : 'AI:Gemini' :");
            var geminiApiConfig = new GeminiApiConfiguration(configuration)
                ?? throw new InvalidOperationException("AI:Gemini section is missing or invalid in appsettings.json, it should be { \"AI\": { \"Gemini\": { \"Version\": \"something\", \"ApiKey\": \"something\" } }}");

            logger.Information(JsonSerializer.Serialize(geminiApiConfig, JsonSerializerOptions));

            services.AddSingleton(geminiApiConfig);

            // services registration
            services.AddScoped<IGenAiApiAdapter, GenAiApiAdapter>();
            addHttpClientCb();
        }
    }
}
