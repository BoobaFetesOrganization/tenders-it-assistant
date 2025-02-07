using GenAIChat.Presentation.API.Controllers.Common;

namespace GenAIChat.Presentation.API.Controllers.Dto
{
    public class DocumentBaseDto : EntityBaseWithNameDto
    {
    }

    public class DocumentDto : DocumentBaseDto
    {
        public byte[] Content { get; set; } = [];
        public string MimeType { get; set; } = string.Empty;
    }
}
