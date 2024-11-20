using GenAIChat.Presentation.API.Controllers.Document;
using GenAIChat.Presentation.API.Controllers.UserStory;

namespace GenAIChat.Presentation.API.Controllers.Project
{
    public class ProjectDto : ProjectBaseDto
    {
        public int ResponseId { get; set; } = 0;

        public IEnumerable<DocumentItemDto> Documents { get; private set; } = [];

        public IEnumerable<UserStoryItemDto> UserStories { get; private set; } = [];
    }
}
