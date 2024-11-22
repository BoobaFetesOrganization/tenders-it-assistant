using GenAIChat.Presentation.API.Controllers.Document;
using GenAIChat.Presentation.API.Controllers.UserStory;

namespace GenAIChat.Presentation.API.Controllers.Project
{
    public class ProjectDto : ProjectBaseDto
    {
        public string Prompt { get; set; } = string.Empty;

        public int ResponseId { get; set; } = 0;

        public IEnumerable<DocumentBaseDto> Documents { get; private set; } = [];

        public IEnumerable<UserStoryBaseDto> UserStories { get; private set; } = [];
    }
}
