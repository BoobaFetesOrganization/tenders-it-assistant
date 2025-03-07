using Microsoft.Extensions.Configuration;

namespace TendersITAssistant.Infrastructure.Api.Gemini.Configuation
{
    public class GeminiApiConfiguration
    {
        public GeminiApiConfiguration(IConfiguration configuration)
        {
            var aiConfig = configuration.GetSection("AI")
                ?? throw new Exception("AI Section not found in the configuration");

            Version = aiConfig.GetValue<string>("Gemini_Version")
                ?? throw new Exception("Gemini_Version property not found in AI Section in the configuration");
            ApiKey = aiConfig.GetValue<string>("Gemini_ApiKey")
                ?? throw new Exception("Gemini_ApiKey property not found in AI Section in the configuration");

        }
        public string Version { get; private set; } = string.Empty;
        public string ApiKey { get; private set; } = string.Empty;
    }
}
