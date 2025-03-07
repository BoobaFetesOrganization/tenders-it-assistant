namespace TendersITAssistant.Presentation.API.Controllers.Dto
{

    public class DocumentDto : DocumentBaseDto
    {
        public byte[] Content { get; set; } = [];
        public string MimeType { get; set; } = string.Empty;
    }
}
