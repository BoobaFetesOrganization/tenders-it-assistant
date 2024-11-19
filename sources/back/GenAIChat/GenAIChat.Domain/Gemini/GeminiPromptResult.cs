using System.Text.Json.Serialization;
using GenAIChat.Domain.Gemini.GeminiResult;

namespace GenAIChat.Domain.Gemini
{
    public class GeminiPromptResult
    {
        [JsonPropertyName("candidates")]
        public IEnumerable<GeminiCandidate> Candidates { get; set; }

        [JsonPropertyName("usageMetadata")]
        public GeminiUsageMetadata UsageMetadata { get; set; }

        [JsonPropertyName("modelVersion")]
        public string ModelVersion { get; set; }
    }
}
