using GenAIChat.Application.Adapter.Api;
using GenAIChat.Infrastructure.Api.Gemini.Configuation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace GenAIChat.Infrastructure.Api.Gemini
{
    public static class ConfigureService
    {
        public static void AddGenAiChatApiServices(this IServiceCollection services, IConfiguration configuration, Action addHttpClientCb)
        {
            // app settings configuration 
            Console.WriteLine("Infrastructure.Api.Gemini : Configuration : 'AI:Gemini' :");
            var geminiApiConfig = configuration.GetSection("AI:Gemini").Get<GeminiApiConfiguration>()
                ?? throw new InvalidOperationException("AI:Gemini section is missing or invalid in appsettings.json, it should be { \"AI\": { \"Gemini\": { \"Version\": \"something\", \"ApiKey\": \"something\" } }}");

            Console.WriteLine(JsonSerializer.Serialize(geminiApiConfig, new JsonSerializerOptions { WriteIndented = true }));


            services.AddSingleton(geminiApiConfig);

            // services registration
            services.AddScoped<IGenAiApiAdapter, GenAiApiAdapter>();
            addHttpClientCb();
            //// ne pas oublier de rajouter les services des API dans la configuration du pressenter....
            // services.AddHttpClient<GeminiGenerateContentService>();
            // services.AddHttpClient<GeminiFileService>();
        }
    }
}
