using GenAIChat.Presentation.API.Controllers.Common;

namespace GenAIChat.Presentation.API.Controllers.Dto
{
    public class UserStoryBaseDto : EntityBaseWithNameDto
    {
        public double Cost { get; set; }
        public IEnumerable<TaskBaseDto> Tasks { get; set; } = [];
    }

    public class UserStoryDto : EntityBaseWithNameDto
    {
        public double Cost { get; set; }
        public IEnumerable<TaskDto> Tasks { get; set; } = [];
    }
}
