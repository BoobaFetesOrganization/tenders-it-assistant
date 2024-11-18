using GenAIChat.Domain.Project;
using System.Threading.Tasks;

namespace GenAIChat.Presentation.API.Controllers.Response.Project
{
    public class ProjectCreateDto
    {
        public string Name { get; set; }
        public IEnumerable<UserStoryDomain> Userstories { get; private set; } = new List<UserStoryDomain>();

        public double Cost { get => Userstories.Sum(us => us.Cost); }
    }
}