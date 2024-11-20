using System.ComponentModel.DataAnnotations;

namespace GenAIChat.Presentation.API.Controllers.Project.Request
{
    public class ProjectUpdateRequest
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Prompt { get; set; }
    }
}
