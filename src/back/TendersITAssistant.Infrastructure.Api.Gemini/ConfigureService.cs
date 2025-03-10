using TendersITAssistant.Application.Adapter.Api;
using TendersITAssistant.Infrastructure.Api.Gemini.Configuation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace TendersITAssistant.Infrastructure.Api.Gemini
{
    public static class ConfigureService
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new() { WriteIndented = true };

        public static void AddInfrastructureApiGemini(this IServiceCollection services, IConfiguration configuration, Action addHttpClientCb, Action<string>? writeLine = null)
        {
            writeLine?.Invoke("configure Infrastructure : Api : Gemini Api services");

            // app settings configuration 
            writeLine?.Invoke("Infrastructure.Api.Gemini : Configuration : 'AI:Gemini' :");
            var geminiApiConfig = new GeminiApiConfiguration(configuration)
                ?? throw new InvalidOperationException("AI:Gemini section is missing or invalid in appsettings.json, it should be { \"AI\": { \"Gemini\": { \"Version\": \"something\", \"ApiKey\": \"something\" } }}");

            writeLine?.Invoke(JsonSerializer.Serialize(geminiApiConfig, JsonSerializerOptions));

            services.AddSingleton(geminiApiConfig);

            // services registration
            services.AddScoped<IGenAiApiAdapter, GenAiApiAdapter>();
            addHttpClientCb();
        }
    }
}
