using GenAIChat.Domain.Common;

namespace GenAIChat.Presentation.API.Controllers.Project
{
    public class ProjectBaseDto : IEntityDomain
    {
        public required int Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public string Prompt { get; set; } = string.Empty;
    }
}
