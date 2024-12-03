using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project.Group;

namespace GenAIChat.Domain.Project
{

    public class ProjectDomain : IEntityDomain
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<DocumentDomain> Documents { get; private set; } = [];

        public UserStoryGroupDomain? Stories { get; set; } = null;

        public ICollection<UserStoryGroupDomain> Generated { get; private set; } = [];

        public ProjectDomain() { }
        public ProjectDomain(string name) : this()
        {
            Name = name;
        }
        public ProjectDomain(int id, string name)
            : this(name)
        {
            Id = id;
        }
    }
}