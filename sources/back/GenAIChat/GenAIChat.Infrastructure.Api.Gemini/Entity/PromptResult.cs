using GenAIChat.Infrastructure.Api.Gemini.Entity.Result;
using System.Text.Json.Serialization;

namespace GenAIChat.Infrastructure.Api.Gemini.Entity
{
    public class PromptResult
    {
        [JsonPropertyName("candidates")]
        public IEnumerable<Candidate> Candidates { get; set; }

        [JsonPropertyName("usageMetadata")]
        public UsageMetadata UsageMetadata { get; set; }

        [JsonPropertyName("modelVersion")]
        public string ModelVersion { get; set; }
    }
}
