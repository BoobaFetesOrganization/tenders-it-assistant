using GenAIChat.Domain.Common;
using GenAIChat.Domain.Gemini;
using GenAIChat.Domain.Project;
using System.Text.Json;

namespace GenAIChat.Domain.Prompt
{

    public static class PromptDomainExtensions
    {
        public static void LoadTextAsGeminiResult(this PromptDomain project)
        {
            if (project.Payload is null) throw new ArgumentException("Payload is null");

            var promptResult = JsonSerializer.Deserialize<GeminiPromptResult>(project.Payload)
                ?? throw new JsonException("Error while converting the result of the payload");

            var responseText = promptResult.Candidates.Last().Content?.Parts.Last()
                ?? throw new InvalidOperationException("Error while getting the response text from the payload");

            project.Text = responseText.Text
                ?? throw new InvalidOperationException("Error while getting the text result from the payload");
        }
    }
}