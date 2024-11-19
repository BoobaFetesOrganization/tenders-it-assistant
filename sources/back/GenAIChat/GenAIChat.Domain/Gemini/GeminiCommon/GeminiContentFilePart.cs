using System.Text.Json.Serialization;

namespace GenAIChat.Domain.Gemini.GeminiCommon
{
    public class GeminiContentFilePart : IGeminiContentPart
    {
        [JsonPropertyName("file_data")]
        public GeminiContentFilePartData FileData { get; set; }

        public GeminiContentFilePart() { }

        public GeminiContentFilePart(string mimeType, string fileUri)
        {
            FileData = new GeminiContentFilePartData(mimeType, fileUri);
        }
    }
}