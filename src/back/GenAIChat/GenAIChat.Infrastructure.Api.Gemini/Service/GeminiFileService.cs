using GenAIChat.Domain.Document;
using GenAIChat.Infrastructure.Api.Gemini.Configuation;
using GenAIChat.Infrastructure.Api.Gemini.Converter;
using System.Net.Http.Headers;
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

        public async Task UploadAsync(DocumentDomain document, CancellationToken cancellationToken = default)
        {
            // upoad file
            var uploadUrl = await UploadFileMetadataAsync(document, cancellationToken);

            // Assuming the response contains the upload URL
            await UploadFileContentAsync(document, uploadUrl, cancellationToken);
        }

        private async Task<string> UploadFileMetadataAsync(DocumentDomain document, CancellationToken cancellationToken = default)
        {
            // action
            HttpRequestMessage request = new(HttpMethod.Post, Endpoint);
            request.Headers.Add("X-Goog-Upload-Protocol", "resumable");
            request.Headers.Add("X-Goog-Upload-Command", "start");
            request.Headers.Add("X-Goog-Upload-Header-Content-Length", document.Metadata.Length.ToString());
            request.Headers.Add("X-Goog-Upload-Header-Content-Type", document.Metadata.MimeType);

            request.Content = new StringContent(JsonSerializer.Serialize(
                new
                {
                    file = new { display_name = document.Name }
                }), System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
                throw new AggregateException(string.Empty, [
                    new Exception("Error while starting the upload to the Gemini API"),
                    new Exception(await response.Content.ReadAsStringAsync(cancellationToken))
                    ]);

            var uploadUrl = response.Headers.GetValues("X-Goog-Upload-URL").FirstOrDefault();
            if (string.IsNullOrEmpty(uploadUrl))
                throw new InvalidOperationException("Upload URL not found in the response headers");


            return uploadUrl;
        }

        private async Task UploadFileContentAsync(DocumentDomain document, string uploadUrl, CancellationToken cancellationToken = default)
        {
            HttpRequestMessage request = new(HttpMethod.Put, uploadUrl)
            {
                Content = new StreamContent(new MemoryStream(document.Content))
            };
            request.Headers.Add("X-Goog-Upload-Offset", "0");
            request.Headers.Add("X-Goog-Upload-Command", "upload, finalize");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(document.Metadata.MimeType);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
                throw new AggregateException(string.Empty, [
                    new Exception("Error while uploading the file to the Gemini API"),
                    new Exception(await response.Content.ReadAsStringAsync(cancellationToken))
                    ]);

            document.Metadata = await UploadFileContentConverter.Convert(response.Content);
        }
    }
}
