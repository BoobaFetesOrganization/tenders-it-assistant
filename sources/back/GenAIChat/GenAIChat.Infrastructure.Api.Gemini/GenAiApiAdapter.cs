using GenAIChat.Application.Adapter.Api;
using GenAIChat.Domain.Document;
using GenAIChat.Domain.Gemini;
using GenAIChat.Domain.Gemini.GeminiCommon;
using GenAIChat.Domain.Prompt;
using GenAIChat.Infrastructure.Api.Gemini.Service;

namespace GenAIChat.Infrastructure.Api.Gemini
{
    public class GenAiApiAdapter(GeminiGenerateContentService generateContentService, GeminiFileService fileService) : IGenAiApiAdapter
    {
        public async Task<PromptDomain> SendPromptAsync(string prompt, IEnumerable<DocumentDomain>? documents = null)
        {
            GeminiPromptData data = new();
            GeminiContent content = new();
            data.Contents.Add(content);

            content.Parts.Add(new(prompt));

            if (documents is not null)
                content.Parts.AddRange(
                    documents.Select(document => new GeminiContentPart(document.Metadata.MimeType, document.Metadata.Uri)));


            PromptDomain result = new(await generateContentService.CallAsync(data));
            result.LoadTextAsGeminiResult();

            return result;
        }


        public async Task<IEnumerable<DocumentDomain>> SendFilesAsync(IEnumerable<DocumentDomain>? documents)
        {
            if (documents is null) return [];

            // upload files asynchroneously
            var actions = documents.Select(async document => await fileService.UploadAsync(document));

            // wait for all files to be uploaded
            var results = await Task.WhenAll(actions);

            return results;
        }
    }
}
