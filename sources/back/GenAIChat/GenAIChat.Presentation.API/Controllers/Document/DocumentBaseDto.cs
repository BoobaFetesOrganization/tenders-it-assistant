using GenAIChat.Domain.Common;

namespace GenAIChat.Presentation.API.Controllers.Document
{
    public class DocumentBaseDto : IEntityDomain
    {
        public required int Id { get; set; }
        public required string Name { get; set; } = string.Empty;
    }
}
