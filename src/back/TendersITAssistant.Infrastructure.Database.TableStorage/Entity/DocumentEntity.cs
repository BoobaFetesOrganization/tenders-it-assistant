﻿using TendersITAssistant.Domain.Document;
using TendersITAssistant.Infrastructure.Database.TableStorage.Entity.Common;

namespace TendersITAssistant.Infrastructure.Database.TableStorage.Entity
{
    internal class DocumentEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string MetadataId { get; set; } =string.Empty;

        #region  navigation properties
        public string ProjectId { get; set; } = string.Empty;

        #endregion
    }
}
