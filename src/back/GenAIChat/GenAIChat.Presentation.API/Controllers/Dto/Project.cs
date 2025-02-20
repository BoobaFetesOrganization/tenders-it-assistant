using GenAIChat.Presentation.API.Controllers.Common;

namespace GenAIChat.Presentation.API.Controllers.Dto
{
    public class ProjectBaseDto : EntityBaseWithNameDto
    {
    }

    public class ProjectDto : ProjectBaseDto
    {
        public IEnumerable<DocumentBaseDto> Documents { get; private set; } = [];
        public UserStoryGroupDto? SelectedGroup { get; private set; } = null;
    }
}
