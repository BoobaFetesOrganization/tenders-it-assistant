using System.Text.Json.Serialization;

namespace GenAIChat.Domain.Gemini.GeminiResult
{
    public class GeminiSafetyRating
    {
        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("probability")]
        public string Probability { get; set; }
    }
}