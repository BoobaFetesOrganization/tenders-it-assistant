using GenAIChat.Presentation.API.Controllers.Common;

namespace GenAIChat.Presentation.API.Controllers.Dto
{
    public class DocumentMetadataBaseDto : EntityBaseWithNameDto
    {
    }

    public class DocumentMetadataDto : DocumentMetadataBaseDto
    {
        public string DisplayName { get; init; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;
        public long Length { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime ExpirationTime { get; set; }
        public string Sha256Hash { get; set; } = string.Empty;
        public string Uri { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }

}
