namespace GenAIChat.Presentation.API.Controllers.Document
{
    public class DocumentDto : DocumentBaseDto
    {
        public required byte[] Content { get; set; } = [];
        public required DateTime CreateTime { get; set; }
        public required DateTime UpdateTime { get; set; }
    }
}
