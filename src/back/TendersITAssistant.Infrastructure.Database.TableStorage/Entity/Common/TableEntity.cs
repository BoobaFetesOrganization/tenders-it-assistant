using Azure;
using Azure.Data.Tables;

namespace TendersITAssistant.Infrastructure.Database.TableStorage.Entity.Common
{
    internal abstract class BaseEntity : ITableEntity
    {

        #region for mapping with IEntityDomain
        public string Id
        {
            get => TableStorageTools.GetId(PartitionKey, RowKey);
            set
            {
                var (partitionKey, rowKey) = TableStorageTools.ExtractKeys(value);
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
