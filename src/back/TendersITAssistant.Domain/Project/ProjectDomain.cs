using TendersITAssistant.Domain.Common;
using TendersITAssistant.Domain.Document;
using TendersITAssistant.Domain.Project.Group;

namespace TendersITAssistant.Domain.Project
{
    public class ProjectDomain : EntityDomain
    {
        public string Name { get; set; } = string.Empty;

        public ICollection<DocumentDomain> Documents { get; set; } = [];

        public string? SelectedGroupId { get; set; } = null;

        public ICollection<UserStoryGroupDomain> Groups { get; set; } = [];
    }
}