using System.ComponentModel.DataAnnotations;

namespace GenAIChat.Presentation.API.Controllers.Prompt.Request
{
    public class PromptSendRequest
    {
        [Required]
        public string Prompt { get; set; } = string.Empty;
        public List<IFormFile> Files { get; set; } = [];
    }
}
