using System.Text.Json.Serialization;

namespace TendersITAssistant.Domain.Gemini.GeminiCommon
{
    public class GeminiContentPart
    {
        // properties for the prompt
        [JsonPropertyName("text")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Text { get; set; } = null;

        // properties for the files
        [JsonPropertyName("file_data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public GeminiContentFilePartData? FileData { get; set; } = null;

        public GeminiContentPart() { }
        public GeminiContentPart(string request) { Text = request; }
        public GeminiContentPart(string mimeType, string fileUri) { FileData = new GeminiContentFilePartData(mimeType, fileUri); }
    }
}