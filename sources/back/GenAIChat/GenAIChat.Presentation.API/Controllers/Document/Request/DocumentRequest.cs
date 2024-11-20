using System.ComponentModel.DataAnnotations;

namespace GenAIChat.Presentation.API.Controllers.Document.Request
{
    public class DocumentRequest
    {
        [Required]
        public required IFormFile File { get; set; }
    }
}
