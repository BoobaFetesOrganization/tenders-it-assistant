using System.Text.Json.Serialization;

namespace TendersITAssistant.Domain.Gemini.GeminiResult
{
    public class GeminiSafetyRating
    {
        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("probability")]
        public string Probability { get; set; } = string.Empty;
    }
}