namespace GenAIChat.Domain.Common
{
    public interface IEntityDomain
    {
        string Id { get; set; }
        DateTimeOffset? Timestamp { get; set; }
    }

    /// <summary>
    /// <list type="bullet">
    /// <item>The PartitionKey property is used to store the partition key of the entity. this is the unique identifier of the entity, the 'id'</item>
    /// <item>The RowKey property is used to store the row key of the entity.</item>
    /// <item>The Timestamp property is used to store the timestamp of the entity.</item>
    /// <item>The ETag property is used to store the entity tag of the entity.</item>      
    /// </list>
    /// </summary>
    public abstract class EntityDomain : IEntityDomain
    {
        public static string NewId() => Guid.NewGuid().ToString();
        public string Id { get; set; } = String.Empty;
        public DateTimeOffset? Timestamp { get; set; }

        public EntityDomain() { }
        public EntityDomain(EntityDomain domain)
        {
            Id = domain.Id;
        }
    }
}
