namespace GenAIChat.Infrastructure.Api.Gemini.Entity.Common
{
    public class ContentFilePartData
    {
        public string mime_type { get; set; } = string.Empty;
        public string file_uri { get; set; } = string.Empty;

        public ContentFilePartData() { }

        public ContentFilePartData(string mimeType, string fileUri)
        {
            mime_type = mimeType;
            file_uri = fileUri;
        }
    }
}
