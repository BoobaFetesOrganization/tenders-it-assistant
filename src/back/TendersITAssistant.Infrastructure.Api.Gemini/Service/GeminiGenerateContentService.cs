using TendersITAssistant.Domain.Gemini;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TendersITAssistant.Infrastructure.Api.Gemini.Configuation;

namespace TendersITAssistant.Infrastructure.Api.Gemini.Service
{

    public class GeminiGenerateContentService
    {
        private readonly HttpClient _httpClient;
        private readonly GeminiApiConfiguration _apiConfiguration;
        private string Endpoint { get => $"https://generativelanguage.googleapis.com/v1beta/models/{_apiConfiguration.Version}:generateContent?key={_apiConfiguration.ApiKey}"; }

        public GeminiGenerateContentService(HttpClient httpClient, GeminiApiConfiguration apiConfiguration)
        {
            _apiConfiguration = apiConfiguration;

            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> CallAsync(GeminiRequest data, CancellationToken cancellationToken = default)
        {
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Endpoint, content, cancellationToken);

            var result = await response.Content.ReadAsStringAsync(cancellationToken);
            if (!response.IsSuccessStatusCode)
                throw new AggregateException(string.Empty, [
                    new Exception("Error while calling the Gemini GenerateContent API"),
                    new Exception(result)
                    ]);

            return result;
        }
    }
}
