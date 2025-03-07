using TendersITAssistant.Presentation.API.Controllers.Common;

namespace TendersITAssistant.Presentation.API.Controllers.Dto
{
    public class UserStoryRequestDto : EntityBaseDto
    {
        public string GroupId { get; set; } = string.Empty;
        public string Context { get; set; } = string.Empty;
        public string Personas { get; set; } = string.Empty;
        public string Tasks { get; set; } = string.Empty;
    }
}
