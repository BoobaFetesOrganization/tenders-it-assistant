namespace GenAIChat.Presentation.API.Controllers.Dto
{

    public class ProjectDto : ProjectBaseDto
    {
        public IEnumerable<DocumentBaseDto> Documents { get; private set; } = [];
        public UserStoryGroupDto? SelectedGroup { get; private set; } = null;
    }
}
