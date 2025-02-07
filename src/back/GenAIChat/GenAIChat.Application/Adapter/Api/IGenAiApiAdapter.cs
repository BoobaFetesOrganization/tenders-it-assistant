using GenAIChat.Domain.Document;
using GenAIChat.Domain.Gemini;

namespace GenAIChat.Application.Adapter.Api
{
    public interface IGenAiApiAdapter
    {
        Task<string> SendRequestAsync(GeminiRequest request);

        Task<IEnumerable<DocumentDomain>> SendFilesAsync(IEnumerable<DocumentDomain> documents, Func<DocumentDomain,Task>? onSent = null);
    }
}
