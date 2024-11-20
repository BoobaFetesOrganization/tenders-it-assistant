namespace GenAIChat.Application.Entity
{
    public class CsvUserStory
    {
        public required string UserStoryId { get; set; }
        public required string UserStoryName { get; set; }
        public required string TaskId { get; set; }
        public required string TaskName { get; set; }
        public double Cost { get; set; }
    }
}