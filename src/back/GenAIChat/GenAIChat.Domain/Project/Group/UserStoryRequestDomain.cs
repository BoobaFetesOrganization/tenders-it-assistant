using GenAIChat.Domain.Common;
using System.Text;

namespace GenAIChat.Domain.Project.Group
{
    public class UserStoryRequestDomain : EntityDomain
    {
        public string Context { get; set; } = string.Empty;
        public string Personas { get; set; } = string.Empty;
        public string Tasks { get; set; } = string.Empty;

        #region  navigation properties
        public string GroupId { get; set; } = string.Empty;
        #endregion


        public override object Clone()
        {
            UserStoryRequestDomain clone = new()
            {
                Context = Context,
                Personas = Personas,
                Tasks = Tasks,
                GroupId = GroupId
            };

            return clone;
        }
    }
}
