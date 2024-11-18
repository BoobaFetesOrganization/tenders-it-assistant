using GenAIChat.Domain.Project;
using System.Threading.Tasks;

namespace GenAIChat.Presentation.API.Controllers.Response.Project
{
    public class ProjectAllDto
    {
        public IEnumerable<ProjectDto> Projects { get; private set; } = [];
    }
}