using GenAIChat.Application.Adapter;
using GenAIChat.Domain;
using GenAIChat.Infrastructure.Api.Gemini.Entity;
using GenAIChat.Infrastructure.Api.Gemini.Entity.Common;
using GenAIChat.Infrastructure.Api.Gemini.Service;

namespace GenAIChat.Infrastructure.Api.Gemini
{
    public class GenAiApiAdapter(GeminiGenerateContentService generateContentService, GeminiFileService fileService) : IGenAiApiAdapter
    {
        public async Task<PromptDomain> SendPromptAsync(string prompt, IEnumerable<DocumentDomain>? documents = null)
        {
            PromptData data = new();
            Content content = new();
            data.contents.Add(content);

            content.parts.Add(new(prompt));

            if (documents is not null) 
                content.parts.AddRange(
                    documents.Select(document => new ContentPart(document.Metadata.MimeType, document.Metadata.Uri)));

            var result = await generateContentService.CallAsync(data);
            var responseText = result.Candidates.Last().content?.parts.Last();

            string text = string.Empty;
            if (responseText?.Text is not null)
            {
                text = responseText.Text;
                responseText.Text = "ref: text";
            }

            return new PromptDomain { Text = text, Payload = result };
        }

        public async Task SendFilesAsync(IEnumerable<DocumentDomain> documents)
        {
            // upload files asynchroneously
            var actions = documents.Select(async document => await fileService.UploadAsync(document));            

            // wait for all files to be uploaded
            var asyncResult = await Task.WhenAll(actions);
        }
    }
}
