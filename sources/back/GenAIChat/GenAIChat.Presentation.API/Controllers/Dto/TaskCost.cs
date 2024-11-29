using GenAIChat.Domain.Project.Group.UserStory.Task.Cost;
using GenAIChat.Presentation.API.Controllers.Common;

namespace GenAIChat.Presentation.API.Controllers.Dto
{
    public class TaskCostDto : EntityBaseDto
    {
        public TaskCostKind Kind { get; set; } = TaskCostKind.Gemini;
        public double Cost { get; set; } = 0;
    }
}
