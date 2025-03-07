using TendersITAssistant.Presentation.API.Controllers.Common;

namespace TendersITAssistant.Presentation.API.Controllers.Dto
{
    public class TaskBaseDto : EntityBaseWithNameDto
    {
        public string UserStoryId { get; set; } = string.Empty;
        public double Cost { get; set; } = 0;
    }
}
