namespace TendersITAssistant.Domain
{
    public class DomainTools
    {
        public static string NewId() => Guid.NewGuid().ToString();
    }
}
