using GenAIChat.Domain.Common;
using GenAIChat.Domain.Document;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenAIChat.Domain
{
    public class DocumentDomain : IEntityDomain
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public DocumentMetadataDomain Metadata { get; set; } = new DocumentMetadataDomain();

        [NotMapped]
        public readonly byte[] Content;
        [NotMapped]
        public string? UploadUrl { get; set; }

        public DocumentDomain() { }

        public DocumentDomain(string filename) : this()
        {
            Name = filename;
        }

        public DocumentDomain(string filename, string contentType, long length, byte[] content) : this(filename)
        {
            Metadata.MimeType = contentType;
            Metadata.SizeBytes = length;
            Content = content;

        }
    }
}
