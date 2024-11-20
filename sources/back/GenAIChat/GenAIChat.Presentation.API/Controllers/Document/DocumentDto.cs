namespace GenAIChat.Presentation.API.Controllers.Document
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte[] Content { get; set; } = [];

        public string MimeType { get; set; } = string.Empty;
        public long Length { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
