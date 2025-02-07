using GenAIChat.Presentation.API.Controllers.Common;

namespace GenAIChat.Presentation.API.Controllers.Dto
{
    public class UserStoryGroupBaseDto : EntityBaseDto
    {
        public int ProjectId { get; set; }
    }

    public class UserStoryGroupDto : EntityBaseDto
    {
        public int ProjectId { get; set; }
        public UserStoryPromptDto Request { get; set; } = new UserStoryPromptDto();
        public string? Response { get; set; } = null;
        public IEnumerable<UserStoryDto> UserStories { get; set; } = [];
    }

}
