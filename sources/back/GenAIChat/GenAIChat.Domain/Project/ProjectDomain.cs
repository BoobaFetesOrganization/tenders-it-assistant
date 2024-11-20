using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Prompt;

namespace GenAIChat.Domain.Project
{

    public class ProjectDomain : IEntityDomain
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Prompt { get; set; } = string.Empty;
        public PromptDomain PromptResponse { get; set; } = new PromptDomain();

        public ICollection<DocumentDomain> Documents { get; private set; } = [];

        public ICollection<UserStoryDomain> UserStories { get; private set; } = [];

        public ProjectDomain() { }
        public ProjectDomain(string name, string prompt) : this()
        {
            Name = name;
            Prompt = prompt;
        }
        public ProjectDomain(int id, string name, string prompt)
            : this(name, prompt)
        {
            Id = id;
        }

        public void SetUserStories(IEnumerable<UserStoryDomain> userstories) => UserStories = userstories.ToList();

        public void SetDocuments(IEnumerable<DocumentDomain> documents) => Documents = documents.ToList();
    }
}