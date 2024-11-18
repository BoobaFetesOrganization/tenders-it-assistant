namespace GenAIChat.Infrastructure.Api.Gemini.Entity.Common
{
    public class ContentFilePart : IContentPart
    {
        public ContentFilePartData file_data { get; set; }

        public ContentFilePart() { }

        public ContentFilePart(string mimeType, string fileUri)
        {
            file_data = new ContentFilePartData(mimeType, fileUri);
        }
    }
}