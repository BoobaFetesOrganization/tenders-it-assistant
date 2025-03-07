using System.Text.Json;
using System.Text.Json.Serialization;
using TendersITAssistant.Domain.Gemini.GeminiResult;

namespace TendersITAssistant.Domain.Gemini
{
    public class GeminiResponse
    {
        [JsonPropertyName("candidates")]
        public IEnumerable<GeminiCandidate> Candidates { get; set; } = [];

        [JsonPropertyName("usageMetadata")]
        public GeminiUsageMetadata UsageMetadata { get; set; } = new GeminiUsageMetadata();

        [JsonPropertyName("modelVersion")]
        public string ModelVersion { get; set; } = string.Empty;

        public static GeminiResponse LoadFrom(string? jsonResponse)
        {
            var json = jsonResponse ?? throw new ArgumentNullException(nameof(jsonResponse), "Gemini reponse should be set");
            return JsonSerializer.Deserialize<GeminiResponse>(json) ?? throw new JsonException("Error while converting the response");
        }
    }
}
