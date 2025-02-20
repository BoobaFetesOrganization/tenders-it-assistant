using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Project.Group;

namespace GenAIChat.Domain.Project
{

    public class ProjectDomain : EntityDomain
    {
        public string Name { get; set; } = string.Empty;

        public ICollection<DocumentDomain> Documents { get; protected set; } = [];

        public UserStoryGroupDomain? SelectedGroup { get; set; } = null;

        public ICollection<UserStoryGroupDomain> Generated { get; protected set; } = [];

        public override object Clone()
        {
            var clone = new ProjectDomain()
            {
                Name = Name,
                SelectedGroup = SelectedGroup?.Clone() as UserStoryGroupDomain
            };

            foreach (var item in Documents) clone.Documents.Add((DocumentDomain)item.Clone());
            foreach (var item in Generated) clone.Generated.Add((UserStoryGroupDomain)item.Clone());

            return clone;
        }
    }
}