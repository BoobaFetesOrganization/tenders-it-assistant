using GenAIChat.Domain.Project.Group;
using GenAIChat.Infrastructure.Database.TableStorage.Entity.Common;

namespace GenAIChat.Infrastructure.Database.TableStorage.Entity
{
    internal class ProjectEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? SelectedGroupId { get; set; } = string.Empty;
    }
}