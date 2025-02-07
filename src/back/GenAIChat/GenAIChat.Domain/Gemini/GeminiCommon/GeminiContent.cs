using System.Text.Json.Serialization;

namespace GenAIChat.Domain.Gemini.GeminiCommon
{
    public class GeminiContent
    {
        [JsonPropertyName("role")] 
        public string? Role { get; set; }

        [JsonPropertyName("parts")] 
        public List<GeminiContentPart> Parts { get; set; } = new List<GeminiContentPart>();

        public void AddPart(GeminiContentPart part)
        {
            Parts.Add(part);
        }
        public void AddParts(IEnumerable<GeminiContentPart> parts)
        {
            Parts.AddRange(parts);
        }
    }
}