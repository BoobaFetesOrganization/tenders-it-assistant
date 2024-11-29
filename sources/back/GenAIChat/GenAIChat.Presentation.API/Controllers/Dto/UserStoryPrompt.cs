using GenAIChat.Presentation.API.Controllers.Common;

namespace GenAIChat.Presentation.API.Controllers.Dto
{
    public class UserStoryPromptDto : EntityBaseDto
    {
        public string Context { get; set; } = string.Empty;
        public string Personas { get; set; } = string.Empty;
        public string Tasks { get; set; } = string.Empty;
        public string Results { get; set; } = string.Empty;
    }
}
