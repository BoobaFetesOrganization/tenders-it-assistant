namespace GenAIChat.Domain
{
    public class DomainTools
    {
        public static string NewId() => Guid.NewGuid().ToString();
    }
}
