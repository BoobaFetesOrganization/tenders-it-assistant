using GenAIChat.Domain;
using GenAIChat.Domain.Document;
using GenAIChat.Infrastructure.Api.Gemini.Configuation;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace GenAIChat.Infrastructure.Api.Gemini.Service
{

    public class GeminiFileService
    {
        private readonly HttpClient _httpClient;
        private readonly GeminiApiConfiguration _apiConfiguration;
        private string Endpoint { get => $"https://generativelanguage.googleapis.com/upload/v1beta/files?key={_apiConfiguration.ApiKey}"; }

        public GeminiFileService(HttpClient httpClient, GeminiApiConfiguration apiConfiguration)
        {
            _apiConfiguration = apiConfiguration;

            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<DocumentDomain> UploadAsync(DocumentDomain document)
        {
            // upoad file
            await UploadFileMetadataAsync(document);

            // Assuming the response contains the upload URL
            await UploadFileContentAsync(document);

            return document;
        }
        private async Task UploadFileMetadataAsync(DocumentDomain document)
        {
            // action
            HttpRequestMessage request = new(HttpMethod.Post, Endpoint);
            request.Headers.Add("X-Goog-Upload-Protocol", "resumable");
            request.Headers.Add("X-Goog-Upload-Command", "start");
            request.Headers.Add("X-Goog-Upload-Header-Content-Length", document.Metadata.SizeBytes.ToString());
            request.Headers.Add("X-Goog-Upload-Header-Content-Type", document.Metadata.MimeType);

            request.Content = new StringContent(JsonSerializer.Serialize(
                new
                {
                    file = new { display_name = document.Name }
                }), System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Error while starting the upload to the Gemini API");
            }

            document.UploadUrl = response.Headers.GetValues("X-Goog-Upload-URL").FirstOrDefault();
            if (string.IsNullOrEmpty(document.UploadUrl))
            {
                throw new InvalidOperationException("Upload URL not found in the response headers");
            }
        }

        private async Task UploadFileContentAsync(DocumentDomain document)
        {
            HttpRequestMessage request = new(HttpMethod.Put, document.UploadUrl)
            {
                Content = new StreamContent(new MemoryStream(document.Content))
            };
            request.Headers.Add("X-Goog-Upload-Offset", "0");
            request.Headers.Add("X-Goog-Upload-Command", "upload, finalize");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(document.Metadata.MimeType);

            var uploadResponse = await _httpClient.SendAsync(request);
            if (!uploadResponse.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Error while uploading the file to the Gemini API");
            }

            var result = await uploadResponse.Content.ReadFromJsonAsync<DocumentMetadataDomain>()
                ?? throw new InvalidOperationException("Failed to deserialize the response from the Gemini API in charge to upload documents");
            document.Metadata = result;
        }
    }

}
