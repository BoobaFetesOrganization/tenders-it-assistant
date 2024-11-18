using System.Text.Json.Serialization;

namespace GenAIChat.Infrastructure.Api.Gemini.Entity.Common
{
    public class ContentPart
    {
        // properties for the prompt

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Text { get; set; } = null;

        // properties for the files
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ContentFilePartData? FileData { get; set; } = null;

        public ContentPart() { }
        public ContentPart(string prompt) { Text = prompt; }
        public ContentPart(string mimeType, string fileUri) { FileData = new ContentFilePartData(mimeType, fileUri); }
    }
}