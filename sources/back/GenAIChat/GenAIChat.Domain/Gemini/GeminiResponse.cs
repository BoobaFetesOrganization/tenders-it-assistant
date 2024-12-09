using GenAIChat.Domain.Gemini.GeminiResult;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GenAIChat.Domain.Gemini
{
    public class GeminiResponse
    {
        [JsonPropertyName("candidates")]
        public IEnumerable<GeminiCandidate> Candidates { get; set; } = [];

        [JsonPropertyName("usageMetadata")]
        public GeminiUsageMetadata UsageMetadata { get; set; } = new GeminiUsageMetadata();

        [JsonPropertyName("modelVersion")]
        public string ModelVersion { get; set; } = string.Empty;

        public static GeminiResponse LoadFrom(string response)
        {
            return JsonSerializer.Deserialize<GeminiResponse>(response)
                ?? throw new JsonException("Error while converting the response");
        }
    }
}
