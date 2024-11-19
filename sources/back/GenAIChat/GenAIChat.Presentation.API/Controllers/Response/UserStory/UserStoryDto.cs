using GenAIChat.Domain.Common;

namespace GenAIChat.Domain.Project
{
    public class UserStoryDto : IEntityDomain
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Cost { get; set; } = 0;
        public IEnumerable<UserStoryTaskDto> Tasks { get; set; } = [];
    }
}