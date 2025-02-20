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
    public abstract class EntityDomain : IEntityDomain, ICloneable
    {
        private const string DefaultPartitionKey = "GenAIChat";
        private static string NewRowKey() => Guid.NewGuid().ToString();

        [Obsolete]
        public static string NewId() => $"genaiChat|{Guid.NewGuid()}";

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
                if (string.IsNullOrWhiteSpace(value))
                {
                    PartitionKey = DefaultPartitionKey;
                    RowKey = NewRowKey();
                }
                else
                {
                    var parts = value.Split("|");
                    PartitionKey = parts[0];
                    RowKey = parts[1];
                }
            }
        }

        #region ITableEntity
        [NotMapped]
        public string PartitionKey { get; set; } = DefaultPartitionKey;
        [NotMapped]
        public string RowKey { get; set; } = NewRowKey();
        public DateTimeOffset? Timestamp { get; set; }
        [NotMapped]
        public ETag ETag { get; set; }
        #endregion

        public EntityDomain() { }
        public EntityDomain(EntityDomain domain)
        {
            Id = domain.Id;
            RowKey = domain.RowKey;
            Timestamp = domain.Timestamp;
            ETag = domain.ETag;
        }

        public abstract object Clone();
    }
}
