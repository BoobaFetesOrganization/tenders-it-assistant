using GenAIChat.Domain.Project.Group;
using System.Reflection;
using System.Text.Json;

namespace GenAIChat.Application.Resources
{
    public class EmbeddedResource
    {
        public readonly UserStoryPromptDomain UserStoryPrompt;

        private readonly Assembly _assembly;
        private readonly string _namespaceName;

        public EmbeddedResource()
        {
            _assembly = Assembly.GetExecutingAssembly();
            _namespaceName = GetType().Namespace ?? string.Empty;

            UserStoryPrompt = GetResourceAs<UserStoryPromptDomain>("UserStoriesPrompt.json");
        }


        public T GetResourceAs<T>(string resourceName)
        {
            string? content;

            var resourceFullName = $"{_namespaceName}.{resourceName}";
            using (var stream = _assembly.GetManifestResourceStream(resourceFullName))
            {
                if (stream is null) throw new FileNotFoundException($"Resource not found: {resourceFullName}");

                using var reader = new StreamReader(stream);
                content = reader.ReadToEnd();
            }


            if (content is null) throw new FileNotFoundException($"Resource {resourceName} is found but there is no content");

            return JsonSerializer.Deserialize<T>(content)
                ?? throw new JsonException($"The content of {resourceName} can not be converted to UserStoryPromptDomain");
        }
    }
}
