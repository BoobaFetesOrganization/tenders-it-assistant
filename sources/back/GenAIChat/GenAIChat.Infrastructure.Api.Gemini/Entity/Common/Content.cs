namespace GenAIChat.Infrastructure.Api.Gemini.Entity.Common
{
    public class Content
    {
        public string? role { get; set; }
        public List<ContentPart> parts { get; set; } = new List<ContentPart>();
    }
}