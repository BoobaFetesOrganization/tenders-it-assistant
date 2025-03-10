using TendersITAssistant.Presentation.API.Controllers.Common;

namespace TendersITAssistant.Presentation.API.Controllers.Dto
{
    public class UserStoryGroupBaseDto : EntityBaseDto
    {
        public string ProjectId { get; set; } = string.Empty;
    }

}
