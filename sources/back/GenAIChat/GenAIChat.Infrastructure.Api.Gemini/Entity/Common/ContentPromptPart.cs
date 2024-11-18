namespace GenAIChat.Infrastructure.Api.Gemini.Entity.Common
{
    public class ContentPromptPart : IContentPart
    {
        public string text { get; set; } = string.Empty;

        public ContentPromptPart() { }

        public ContentPromptPart(string prompt)
        {
            text = prompt;
        }
    }
}