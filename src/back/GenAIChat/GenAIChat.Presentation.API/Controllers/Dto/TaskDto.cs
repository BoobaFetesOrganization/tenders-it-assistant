namespace GenAIChat.Presentation.API.Controllers.Dto
{

    public class TaskDto : TaskBaseDto
    {
        public IEnumerable<TaskCostDto> WorkingCosts { get; set; } = [];
    }
}
