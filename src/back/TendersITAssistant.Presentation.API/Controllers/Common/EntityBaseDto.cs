namespace TendersITAssistant.Presentation.API.Controllers.Common
{
    public class EntityBaseDto
    {
        public string Id { get; set; } = string.Empty;
        public DateTimeOffset? Timestamp { get; set; } = null;
    }

}
