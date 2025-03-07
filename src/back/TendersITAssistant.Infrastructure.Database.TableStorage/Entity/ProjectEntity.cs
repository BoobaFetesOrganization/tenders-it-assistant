using TendersITAssistant.Infrastructure.Database.TableStorage.Entity.Common;

namespace TendersITAssistant.Infrastructure.Database.TableStorage.Entity
{
    internal class ProjectEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? SelectedGroupId { get; set; } = null;
    }
}