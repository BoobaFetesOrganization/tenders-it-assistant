using System.Text.Json;
using System.Text.Json.Serialization;

namespace GenAIChat.Domain.Gemini.GeminiCommon
{
    public class GeminiGenerationConfig
    {
        public static GeminiGenerationConfig AsJsonResult(string? schema) => new()
        {
            MimeType = "application/json",
            Schema = String.IsNullOrWhiteSpace(schema) 
                ? null 
                : JsonSerializer.Deserialize<object>(schema) 
                    ?? "Failed to deserialize the schema from the Gemini API in charge to generate documents"
        };

        [JsonPropertyName("candidate_count")]
        public int? CandidateCount { get; set; }

        [JsonPropertyName("stop_sequences")]
        public List<string>? StopSequences { get; set; }

        [JsonPropertyName("max_output_tokens")]
        public int? MaxOutputTokens { get; set; }

        [JsonPropertyName("temperature")]
        public float? Temperature { get; set; }

        [JsonPropertyName("top_p")]
        public float? TopP { get; set; }

        [JsonPropertyName("top_k")]
        public int? TopK { get; set; }

        [JsonPropertyName("seed")]
        public int? Seed { get; set; }

        [JsonPropertyName("response_mime_type")]
        public string? MimeType { get; set; }

        [JsonPropertyName("response_schema")]
        public object? Schema { get; set; }

        [JsonPropertyName("presence_penalty")]
        public float? PresencePenalty { get; set; }

        [JsonPropertyName("frequency_penalty")]
        public float? FrequencyPenalty { get; set; }

        [JsonPropertyName("response_logprobs")]
        public bool? ResponseLogprobs { get; set; }

        [JsonPropertyName("logprobs")]
        public int? Logprobs { get; set; }
    }
}
