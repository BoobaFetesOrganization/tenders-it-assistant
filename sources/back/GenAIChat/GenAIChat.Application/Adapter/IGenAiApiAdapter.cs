using GenAIChat.Domain;

namespace GenAIChat.Application.Adapter
{
    public interface IGenAiApiAdapter
    {
        Task<PromptDomain> SendPromptAsync(string prompt, IEnumerable<DocumentDomain>? documents = null);

        Task SendFilesAsync(IEnumerable<DocumentDomain> documents);
    }
}
