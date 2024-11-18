using GenAIChat.Infrastructure.Api.Gemini.Entity.Common;

namespace GenAIChat.Infrastructure.Api.Gemini.Entity.Result
{
    public class Candidate
    {
        public Content content { get; set; }
        public string finishReason { get; set; }
        public int index { get; set; }
        public IEnumerable<SafetyRating> safetyRating { get; set; }
    }
}
