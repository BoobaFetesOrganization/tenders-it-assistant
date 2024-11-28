using GenAIChat.Presentation.API.Controllers.Common;

namespace GenAIChat.Presentation.API.Controllers.Dto
{
    public class DocumentBaseDto : EntityBaseWithNameDto
    {
    }

    public class DocumentDto : DocumentBaseDto
    {
        public byte[] Content { get; set; } = Array.Empty<byte>();
    }
}
