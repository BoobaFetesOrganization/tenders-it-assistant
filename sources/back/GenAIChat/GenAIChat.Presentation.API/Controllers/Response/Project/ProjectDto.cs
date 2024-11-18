using GenAIChat.Domain.Common;

namespace GenAIChat.Presentation.API.Controllers.Response.Project
{
    public class ProjectDto : IEntityDomain
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
