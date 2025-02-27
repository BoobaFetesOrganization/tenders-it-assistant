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

        public override object Clone()
        {
            var clone = new ProjectDomain()
            {
                Name = Name,
                SelectedGroupId = SelectedGroupId
            };

            foreach (var item in Documents) clone.Documents.Add((DocumentDomain)item.Clone());
            foreach (var item in Groups) clone.Groups.Add((UserStoryGroupDomain)item.Clone());

            return clone;
        }
    }
}