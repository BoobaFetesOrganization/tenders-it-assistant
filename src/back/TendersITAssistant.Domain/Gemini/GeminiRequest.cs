using System.Text.Json.Serialization;
using TendersITAssistant.Domain.Gemini.GeminiCommon;

namespace TendersITAssistant.Domain.Gemini
{
    public class GeminiRequest
    {
        [JsonPropertyName("contents")]
        public List<GeminiContent> Contents { get; set; } = new List<GeminiContent>();
        [JsonPropertyName("generationConfig")]
        public GeminiGenerationConfig GenerationConfig { get; set; } = new();

        public void AddContent(GeminiContent content)
        {
            Contents.Add(content);
        }

        public void SetGenerationConfig(GeminiGenerationConfig config)
        {
            GenerationConfig = config;
        }
    }
}
