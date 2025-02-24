using GenAIChat.Domain.Document;
using GenAIChat.Infrastructure.Database.TableStorage.Entity.Common;

namespace GenAIChat.Infrastructure.Database.TableStorage.Entity
{
    internal class DocumentEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public byte[] Content { get; set; } = [];// devrait etre stocké dans un filestorage !!

        public string MetadataId { get; set; } =string.Empty;

        #region  navigation properties
        public string ProjectId { get; set; } = string.Empty;

        #endregion
    }
}
