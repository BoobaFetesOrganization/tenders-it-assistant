using GenAIChat.Domain.Common;

namespace GenAIChat.Presentation.API.Controllers.UserStory
{
    public class UserStoryDto : UserStoryBaseDto
    {
        public IEnumerable<UserStoryTaskDto> Tasks { get; set; } = [];
    }
}