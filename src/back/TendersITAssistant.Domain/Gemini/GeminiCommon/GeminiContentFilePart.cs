using System.Text.Json.Serialization;

namespace TendersITAssistant.Domain.Gemini.GeminiCommon
{
    public class GeminiContentFilePart : IGeminiContentPart
    {
        [JsonPropertyName("file_data")]
        public GeminiContentFilePartData FileData { get; set; } = new GeminiContentFilePartData();

        public GeminiContentFilePart() { }

        public GeminiContentFilePart(string mimeType, string fileUri)
        {
            FileData = new GeminiContentFilePartData(mimeType, fileUri);
        }
    }
}