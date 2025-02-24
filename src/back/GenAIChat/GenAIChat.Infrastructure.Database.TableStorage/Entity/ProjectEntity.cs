using GenAIChat.Infrastructure.Database.TableStorage.Entity.Common;

namespace GenAIChat.Infrastructure.Database.TableStorage.Entity
{
    internal class ProjectEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? SelectedGroupId { get; set; } = string.Empty;

        public ProjectEntity() { }
        public ProjectEntity(string id) : base(id) { }
        public ProjectEntity(string partitionKey, string rowKey) : base(partitionKey, rowKey) { }
    }
}