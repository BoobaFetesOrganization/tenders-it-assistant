using GenAIChat.Domain.Document;
using GenAIChat.Domain.Gemini;

namespace GenAIChat.Application.Adapter.Api
{
    public interface IGenAiApiAdapter
    {
        Task<string> SendRequestAsync(GeminiRequest request, CancellationToken cancellationToken = default);

        Task<IEnumerable<DocumentDomain>> SendFilesAsync(IEnumerable<DocumentDomain> documents, CancellationToken cancellationToken = default);
    }
}
