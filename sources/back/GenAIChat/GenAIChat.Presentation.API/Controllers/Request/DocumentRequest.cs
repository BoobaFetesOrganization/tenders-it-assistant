using System.ComponentModel.DataAnnotations;

namespace GenAIChat.Presentation.API.Controllers.Request
{
    public class DocumentRequest
    {
        [Required]
        public required IFormFile File { get; set; }
    }
}
