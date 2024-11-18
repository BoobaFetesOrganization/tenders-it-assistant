using GenAIChat.Application.Adapter;
using GenAIChat.Domain;

namespace GenAIChat.Application
{
    public interface IPromptApplication
    {
        Task<PromptDomain> SendAsync(string prompt);
    }

    public class PromptApplication : IPromptApplication
    {
        private readonly IGenAiApiAdapter _genAiAdapter;

        public PromptApplication(IGenAiApiAdapter genAiAdapter)
        {
            _genAiAdapter = genAiAdapter;
        }

        public async Task<PromptDomain> SendAsync(string prompt)
        {
            return await _genAiAdapter.SendPromptAsync(prompt);
        }
    }
}
