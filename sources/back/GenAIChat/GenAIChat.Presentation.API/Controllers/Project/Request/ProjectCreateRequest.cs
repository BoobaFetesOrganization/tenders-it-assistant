using System.ComponentModel.DataAnnotations;

namespace GenAIChat.Presentation.API.Controllers.Project.Request
{
    public class ProjectCreateRequest
    {
        [Required]
        public required string Name { get; set; } 

        [Required]
        public ICollection<IFormFile> Files { get; set; } = [];
    }
}
