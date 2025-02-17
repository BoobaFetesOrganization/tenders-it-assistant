using GenAIChat.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenAIChat.Domain.Prompt
{
    public class PromptDomain : EntityDomain
    {
        [NotMapped]
        public string Text { get; set; } = string.Empty;

        public string? Payload { get; set; } = null;

        public PromptDomain() { }
        public PromptDomain(string? payload) : this() => Payload = payload;
    }
}
