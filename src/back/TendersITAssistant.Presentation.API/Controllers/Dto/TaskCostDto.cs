using TendersITAssistant.Domain.Project.Group.UserStory.Task.Cost;
using TendersITAssistant.Presentation.API.Controllers.Common;

namespace TendersITAssistant.Presentation.API.Controllers.Dto
{
    public class TaskCostDto : EntityBaseDto
    {
        public TaskCostKind Kind { get; set; } = TaskCostKind.Gemini;
        public double Cost { get; set; } = 0;
    }
}
