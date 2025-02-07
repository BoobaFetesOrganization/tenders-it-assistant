using GenAIChat.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenAIChat.Domain.Prompt
{
    public class PromptDomain : IEntityDomain
    {
        public int Id { get; set; }

        [NotMapped]
        public string Text { get; set; } = string.Empty;

        public string? Payload { get; set; } = null;

        public PromptDomain() { }
        public PromptDomain(string? payload) => Payload = payload;
    }
}
