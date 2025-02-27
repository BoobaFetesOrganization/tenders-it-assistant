using GenAIChat.Presentation.API.Controllers.Common;

namespace GenAIChat.Presentation.API.Controllers.Dto
{

    public class UserStoryGroupDto : EntityBaseDto
    {
        public string ProjectId { get; set; }
        public UserStoryRequestDto Request { get; set; } = new UserStoryRequestDto();
        public string? Response { get; set; } = null;
        public IEnumerable<UserStoryDto> UserStories { get; set; } = [];
    }

}
