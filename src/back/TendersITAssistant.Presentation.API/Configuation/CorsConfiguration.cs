using Microsoft.AspNetCore.Mvc;

namespace TendersITAssistant.Presentation.API.Configuation
{
    public class CorsConfiguration
    {
        public required string Name { get; set; }
        public IEnumerable<string>? Origins { get; set; } = null;
        public IEnumerable<string>? AllowedVerbs { get; set; } = null;
        public IEnumerable<string>? AllowedHeaders { get; set; } = null;
    }
}
