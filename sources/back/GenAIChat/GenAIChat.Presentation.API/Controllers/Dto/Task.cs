using GenAIChat.Presentation.API.Controllers.Common;

namespace GenAIChat.Presentation.API.Controllers.Dto
{
    public class TaskBaseDto : EntityBaseWithNameDto
    {
        public int UserStoryId { get; set; }
        public double Cost { get; set; } = 0;
    }

    public class TaskDto : TaskBaseDto
    {
        public IEnumerable<TaskCostDto> WorkingCosts { get; set; } = [];
    }
}
