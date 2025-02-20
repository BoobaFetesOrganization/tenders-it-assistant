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

        public override object Clone()
        {
            DocumentDomain clone = new();

            clone.Name = Name;
            clone.Content = Content;
            clone.Metadata = (DocumentMetadataDomain)Metadata.Clone();
            clone.ProjectId = ProjectId;

            return clone;
        }
    }
}
