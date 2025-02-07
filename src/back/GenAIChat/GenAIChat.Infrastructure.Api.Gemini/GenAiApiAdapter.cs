using GenAIChat.Application.Adapter.Api;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Gemini;
using GenAIChat.Domain.Gemini.GeminiCommon;
using GenAIChat.Infrastructure.Api.Gemini.Service;

namespace GenAIChat.Infrastructure.Api.Gemini
{
    public class GenAiApiAdapter(GeminiGenerateContentService generateContentService, GeminiFileService fileService) : IGenAiApiAdapter
    {
        public async Task<string> SendRequestAsync(GeminiRequest request)
        {
            return await generateContentService.CallAsync(request);
        }


        public async Task<IEnumerable<DocumentDomain>> SendFilesAsync(IEnumerable<DocumentDomain>? documents, Func<DocumentDomain, Task>? onSent = null)
        {
            if (documents is null) return [];

            // find expired documents
            var validDocuments = documents.Where(doc => doc.Metadata.ExpirationTime > DateTime.Now);
            var documentToUpload = documents.Except(validDocuments).ToArray();

            // when no expired documents are found, operation should ends as expected
            if (!documentToUpload.Any()) return documents;

            // upload expired documents
            var actions = documentToUpload.Select(async document => await fileService.UploadAsync(document));
            var sentDocuments = await Task.WhenAll(actions);

            // execute action onUpdated 
            if (onSent is not null)
            {
                var onSentActions = sentDocuments.Select(async doc => await onSent(doc));
                await Task.WhenAll(onSentActions);
            }

            return documents;
        }
    }
}
