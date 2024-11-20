using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Prompt;

namespace GenAIChat.Domain.Project
{

    public class ProjectDomain : IEntityDomain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Prompt { get; set; }
        public PromptDomain PromptResponse { get; set; } = new PromptDomain();

        public ICollection<DocumentDomain> Documents { get; private set; } = [];

        public ICollection<UserStoryDomain> UserStories { get; private set; } = [];

        public ProjectDomain() { }
        public ProjectDomain(string name, string prompt, IEnumerable<DocumentDomain>? documents = null) : this()
        {
            Name = name;
            Prompt = prompt;
            if (documents != null) Documents = documents.ToList();
        }
        public ProjectDomain(int id, string name, string prompt, IEnumerable<DocumentDomain>? documents = null)
            : this(name, prompt, documents)
        {
            Id = id;
        }

        public void SetUserStories(IEnumerable<UserStoryDomain> userstories) => UserStories = userstories.ToList();

        public void SetDocuments(IEnumerable<DocumentDomain> documents) => Documents = documents.ToList();
    }
}