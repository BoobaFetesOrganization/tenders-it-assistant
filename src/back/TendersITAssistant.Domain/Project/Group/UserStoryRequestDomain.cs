using TendersITAssistant.Domain.Common;

namespace TendersITAssistant.Domain.Project.Group
{
    public class UserStoryRequestDomain : EntityDomain
    {
        public string Context { get; set; } = string.Empty;
        public string Personas { get; set; } = string.Empty;
        public string Tasks { get; set; } = string.Empty;

        #region  navigation properties
        public string GroupId { get; set; } = string.Empty;
        #endregion
    }
}
