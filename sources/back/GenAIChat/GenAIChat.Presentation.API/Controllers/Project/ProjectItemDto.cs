using GenAIChat.Domain.Common;

namespace GenAIChat.Presentation.API.Controllers.Project
{
    public class ProjectItemDto : IEntityDomain
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
