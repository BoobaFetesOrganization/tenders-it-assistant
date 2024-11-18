using System.ComponentModel.DataAnnotations;

namespace GenAIChat.Presentation.API.Controllers.Request
{
    public class ProjectCreateRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }
}
