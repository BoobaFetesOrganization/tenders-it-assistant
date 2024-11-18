using GenAIChat.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenAIChat.Domain
{
    public class PromptDomain : IEntityDomain
    {
        public int Id { get; set; }

        public string Text { get; set; } = string.Empty;

        [NotMapped]
        public object? Payload { get; set; } = null;
    }
}
