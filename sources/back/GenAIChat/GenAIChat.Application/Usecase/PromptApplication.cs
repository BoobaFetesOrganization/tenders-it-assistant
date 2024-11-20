using GenAIChat.Application.Adapter.Api;
using GenAIChat.Domain.Prompt;

namespace GenAIChat.Application.Usecase
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
