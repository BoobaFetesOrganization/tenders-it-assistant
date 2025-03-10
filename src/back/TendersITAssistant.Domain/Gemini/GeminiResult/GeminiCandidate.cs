using System.Text.Json.Serialization;
using TendersITAssistant.Domain.Gemini.GeminiCommon;

namespace TendersITAssistant.Domain.Gemini.GeminiResult
{
    public class GeminiCandidate
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("content")]
        public GeminiContent Content { get; set; } = new GeminiContent();

        [JsonPropertyName("finishReason")]
        public string FinishReason { get; set; } = string.Empty;

        [JsonPropertyName("safetyRating")]
        public IEnumerable<GeminiSafetyRating> SafetyRating { get; set; } = [];
    }
}
