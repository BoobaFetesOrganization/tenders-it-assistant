using System.Reflection;

namespace GenAIChat.Application.Resources
{
    public class EmbeddedResource
    {
        public const string UserStoryPrompt = "UserStoriesPrompt.txt";

        private readonly Assembly _assembly;
        private readonly string _namespaceName;

        private readonly Dictionary<string, string> _memory = new Dictionary<string, string>();

        public EmbeddedResource()
        {
            _assembly = Assembly.GetExecutingAssembly();
            _namespaceName = GetType().Namespace ?? string.Empty;
        }

        public string? GetAsString(string resourceName)
        {
            try
            {
                var fullResourceName = $"{_namespaceName}.{resourceName}";
                using (var stream = _assembly.GetManifestResourceStream(fullResourceName))
                {
                    if (stream is null) throw new FileNotFoundException($"Resource not found: {fullResourceName}");

                    using (var reader = new StreamReader(stream))
                        return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            return null;
        }

        public string? this[string resourceName]
        {
            get
            {
                if (!_memory.ContainsKey(resourceName))
                {
                    var content = GetAsString(resourceName);
                    if (content is null) return null;
                    _memory.Add(resourceName, content);
                }
                return _memory[resourceName];
            }
        }
    }
}
