using GenAIChat.Domain;
using GenAIChat.Domain.Project;
using GenAIChat.Presentation.API.Controllers.Response.Document;

namespace GenAIChat.Presentation.API.Controllers.Response.Project
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public readonly string Name;
        public readonly string Prompt;
        public int ResponseId { get; set; } = 0;

        public IEnumerable<DocumentItemDto> Documents { get; private set; } = [];

        public IEnumerable<UserStoryItemDto> UserStories { get; private set; } = [];
    }
}
