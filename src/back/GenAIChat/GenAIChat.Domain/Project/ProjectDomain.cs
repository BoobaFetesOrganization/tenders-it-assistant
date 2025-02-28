using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project.Group;

namespace GenAIChat.Domain.Project
{
    public class ProjectDomain : EntityDomain
    {
        public string Name { get; set; } = string.Empty;

        public ICollection<DocumentDomain> Documents { get; protected set; } = [];

        public string? SelectedGroupId { get; set; } = null;

        public ICollection<UserStoryGroupDomain> Groups { get; protected set; } = [];
    }
}