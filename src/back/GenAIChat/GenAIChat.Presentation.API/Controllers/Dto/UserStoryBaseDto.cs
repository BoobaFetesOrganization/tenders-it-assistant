using GenAIChat.Presentation.API.Controllers.Common;

namespace GenAIChat.Presentation.API.Controllers.Dto
{
    public class UserStoryBaseDto : EntityBaseWithNameDto
    {
        public string GroupId { get; set; } = string.Empty;
        public double Cost { get; set; }
        public IEnumerable<TaskBaseDto> Tasks { get; set; } = [];
    }
}
