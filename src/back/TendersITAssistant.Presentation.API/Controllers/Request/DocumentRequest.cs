using System.ComponentModel.DataAnnotations;

namespace TendersITAssistant.Presentation.API.Controllers.Request
{
    public class DocumentRequest
    {
        [Required]
        public required IFormFile File { get; set; }
    }
}
