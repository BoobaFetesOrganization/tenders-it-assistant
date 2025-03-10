using System.Text.Json.Serialization;

namespace TendersITAssistant.Domain.Gemini.GeminiResult
{
    public class GeminiUsageMetadata
    {

        [JsonPropertyName("promptTokenCount")]
        public int PromptTokenCount { get; set; }

        [JsonPropertyName("candidatesTokenCount")]
        public int CandidatesTokenCount { get; set; }

        [JsonPropertyName("totalTokenCount")]
        public int TotalTokenCount { get; set; }
    }
}