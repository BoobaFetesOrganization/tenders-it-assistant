using Azure;
using Azure.Data.Tables;

namespace GenAIChat.Infrastructure.Database.TableStorage.Entity.Common
{
    internal abstract class BaseEntity : ITableEntity
    {

        #region for mapping with IEntityDomain
        public string Id
        {
            get => Tools.GetId(PartitionKey, RowKey);
            set
            {
                var (partitionKey, rowKey) = Tools.ExtractKeys(value);
                PartitionKey = partitionKey;
                RowKey = rowKey;
            }
        }
        #endregion

        #region ITableEntity
        public string PartitionKey { get; set; } = string.Empty;
        public string RowKey { get; set; } = string.Empty;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        #endregion
    }
}
