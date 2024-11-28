using GenAIChat.Domain.Document;

namespace GenAIChat.Application.Adapter.Api
{
    public interface IGenAiApiAdapter
    {
        Task<string> SendRequestAsync(string request, IEnumerable<DocumentDomain>? documents = null);

        Task<IEnumerable<DocumentDomain>> SendFilesAsync(IEnumerable<DocumentDomain> documents, Func<DocumentDomain,Task>? onSent = null);
    }
}
