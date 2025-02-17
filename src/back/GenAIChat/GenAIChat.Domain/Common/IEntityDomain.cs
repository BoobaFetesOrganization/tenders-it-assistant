using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenAIChat.Domain.Common
{
    public interface IEntityDomain : ITableEntity
    {
        string Id { get; set; }
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
        public static string NewId() => $"genaiChat|{Guid.NewGuid()}";
        public static (string partitionKey, string rowKey) ExtractKeys(IEntityDomain domain) => ExtractKeys(domain.Id);
        public static (string partitionKey, string rowKey) ExtractKeys(string id)
        {
            var ids = id.Split('|');
            return (ids[0], ids[1]);
        }
        public static void SetNewTimeStamp(IEntityDomain domain) => domain.Timestamp = DateTimeOffset.Now;

        /// <summary>
        /// The Id property is used to store the unique identifier of the entity. 
        /// It is compopsed by the partition key and the row key separated by a pipe '|'
        /// It's allow to get and set the partition key and row key from the Id property and use different kind of database (like Azure TableStorage as nosql databases or PostGres as sql databases)
        /// </summary>
        public string Id
        {
            get => $"{PartitionKey}|{RowKey}";
            set
            {
                var (partitionKey, rowKey) = ExtractKeys(value);
                PartitionKey = partitionKey;
                RowKey = rowKey;
            }
        }

        #region ITableEntity
        [NotMapped]
        public string PartitionKey { get; set; } = "GenAIChat";
        [NotMapped]
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset? Timestamp { get; set; } = DateTimeOffset.Now;
        [NotMapped] 
        public ETag ETag { get; set; } = new ETag();
        #endregion

        public EntityDomain() { }
        public EntityDomain(EntityDomain domain)
        {
            Id = domain.Id;
            RowKey = domain.RowKey;
            Timestamp = domain.Timestamp;
            ETag = domain.ETag;
        }
    }
}
