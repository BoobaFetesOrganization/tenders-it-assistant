using GenAIChat.Domain.Gemini.GeminiCommon;
using System.Text.Json.Serialization;

namespace GenAIChat.Domain.Gemini
{
    public class GeminiPromptData
    {
        [JsonPropertyName("contents")]
        public List<GeminiContent> Contents { get; set; } = new List<GeminiContent>();

    }
}
