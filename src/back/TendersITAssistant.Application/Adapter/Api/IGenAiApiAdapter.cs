using TendersITAssistant.Domain.Document;
using TendersITAssistant.Domain.Gemini;

namespace TendersITAssistant.Application.Adapter.Api
{
    public interface IGenAiApiAdapter
    {
        Task<string> SendRequestAsync(GeminiRequest request, CancellationToken cancellationToken = default);

        Task<IEnumerable<DocumentDomain>> SendFilesAsync(IEnumerable<DocumentDomain> documents, CancellationToken cancellationToken = default);
    }
}
