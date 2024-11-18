using GenAIChat.Infrastructure.Api.Gemini.Entity.Common;

namespace GenAIChat.Infrastructure.Api.Gemini.Entity
{
    public class PromptData
    {
        public List<Content> contents { get; set; } = new List<Content>();

    }
}
