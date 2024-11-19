using GenAIChat.Domain.Document;
using GenAIChat.Domain.Prompt;

namespace GenAIChat.Application.Adapter.Api
{
    public interface IGenAiApiAdapter
    {
        Task<PromptDomain> SendPromptAsync(string prompt, IEnumerable<DocumentDomain>? documents = null);

        Task<IEnumerable<DocumentDomain>> SendFilesAsync(IEnumerable<DocumentDomain> documents);
    }
}
