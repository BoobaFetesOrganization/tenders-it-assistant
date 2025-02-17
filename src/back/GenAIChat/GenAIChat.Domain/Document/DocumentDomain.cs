using GenAIChat.Domain.Common;

namespace GenAIChat.Domain.Document
{
    public class DocumentDomain : EntityDomain
    {
        public string Name { get; set; } = string.Empty;
        public byte[] Content { get; set; } = [];

        public DocumentMetadataDomain Metadata { get; set; } = new DocumentMetadataDomain();

        #region  navigation properties
        public string ProjectId { get; set; } = string.Empty;
        #endregion
    }
}
