using Azure;
using Azure.Data.Tables;
using GenAIChat.Domain.Common;

namespace GenAIChat.Infrastructure.Database.TableStorage.Entity.Common
{
    internal abstract class BaseEntity : ITableEntity
    {
        private const string DefaultPartitionKey = "GenAIChat";


        #region for mapping with IEntityDomain
        public string Id { get => $"{PartitionKey}|{RowKey}"; }
        #endregion

        #region ITableEntity
        public string PartitionKey { get; set; } = DefaultPartitionKey;
        public string RowKey { get; set; } = EntityDomain.NewId();
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        #endregion

        protected BaseEntity() { }
        protected BaseEntity(string id) => SetId(id);
        protected BaseEntity(string partitionKey, string rowKey) => SetKeys(partitionKey, rowKey);

        public void SetNewKeys()
        {
            PartitionKey = DefaultPartitionKey;
            RowKey = EntityDomain.NewId();
        }
        public void SetKeys(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }
        public void SetId(string id)
        {
            var parts = string.IsNullOrWhiteSpace(id) ? [] : id.Split("|");
            if (parts.Length < 2) SetNewKeys();
            else SetKeys(parts[0], parts[1]);
        }
    }
}
