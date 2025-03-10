using TendersITAssistant.Presentation.API.Controllers.Common;

namespace TendersITAssistant.Presentation.API.Controllers.Dto
{

    public class UserStoryDto : EntityBaseWithNameDto
    {
        public string GroupId { get; set; } = string.Empty;
        public double Cost { get; set; }
        public IEnumerable<TaskDto> Tasks { get; set; } = [];
    }
}
