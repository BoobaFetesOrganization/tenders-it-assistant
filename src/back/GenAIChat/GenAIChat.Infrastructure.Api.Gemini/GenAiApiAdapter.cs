using GenAIChat.Application.Adapter.Api;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Gemini;
using GenAIChat.Infrastructure.Api.Gemini.Service;

namespace GenAIChat.Infrastructure.Api.Gemini
{
    public class GenAiApiAdapter(GeminiGenerateContentService generateContentService, GeminiFileService fileService) : IGenAiApiAdapter
    {
        public async Task<string> SendRequestAsync(GeminiRequest request, CancellationToken cancellationToken = default)
        {
            return await generateContentService.CallAsync(request, cancellationToken);
        }


        public async Task<IEnumerable<DocumentDomain>> SendFilesAsync(IEnumerable<DocumentDomain> documents, CancellationToken cancellationToken = default)
        {
            // find expired documents
            var uploads = documents.Where(doc => doc.Metadata.ExpirationTime <= DateTime.UtcNow);

            // when no expired documents are found, operation should ends as expected
            if (!uploads.Any()) return [];

            // (parallelism) rehydrate each document
            await Task.WhenAll(uploads.Select(async document => await fileService.UploadAsync(document, cancellationToken)));

            return uploads;
        }
    }
}
