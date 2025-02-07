using System.Text.Json.Serialization;

namespace GenAIChat.Domain.Gemini.GeminiCommon
{
    public class GeminiContentFilePartData
    {
        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; } = string.Empty;

        [JsonPropertyName("file_uri")]
        public string FileUri { get; set; } = string.Empty;

        public GeminiContentFilePartData() { }

        public GeminiContentFilePartData(string mimeType, string fileUri)
        {
            MimeType = mimeType;
            FileUri = fileUri;
        }
    }
}
