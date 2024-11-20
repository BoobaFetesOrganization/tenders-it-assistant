using GenAIChat.Domain.Gemini.GeminiResult;
using System.Text.Json.Serialization;

namespace GenAIChat.Domain.Gemini
{
    public class GeminiPromptResult
    {
        [JsonPropertyName("candidates")]
        public IEnumerable<GeminiCandidate> Candidates { get; set; } = [];

        [JsonPropertyName("usageMetadata")]
        public GeminiUsageMetadata UsageMetadata { get; set; } = new GeminiUsageMetadata();

        [JsonPropertyName("modelVersion")]
        public string ModelVersion { get; set; } = string.Empty;
    }
}
