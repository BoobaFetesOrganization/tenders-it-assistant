namespace GenAIChat.Infrastructure.Api.Gemini.Entity.Result
{
    public class SafetyRating
    {
        public string category { get; set; }
        public string probability { get; set; }
    }
}