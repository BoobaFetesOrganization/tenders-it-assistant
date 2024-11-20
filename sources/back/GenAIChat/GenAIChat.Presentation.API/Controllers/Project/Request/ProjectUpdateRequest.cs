using System.ComponentModel.DataAnnotations;

namespace GenAIChat.Presentation.API.Controllers.Project.Request
{
    public class ProjectUpdateRequest : ProjectCreateRequest
    {
        [Required]
        public required string Prompt { get; set; }
    }
}
