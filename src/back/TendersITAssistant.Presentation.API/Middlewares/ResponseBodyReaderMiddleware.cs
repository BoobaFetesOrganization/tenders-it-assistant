using Serilog;
using System.Text.Json;

namespace TendersITAssistant.Presentation.API.Middlewares
{
    public static class ResponseBodyReaderMiddlewareExtensions
    {
        public static IApplicationBuilder UseResponseBodyReaderMiddleware(this IApplicationBuilder builder) => builder.UseMiddleware<ResponseBodyReaderMiddleware>();
    }

    public class ResponseBodyReaderMiddleware
    {
        private readonly RequestDelegate next;

        public ResponseBodyReaderMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, IDiagnosticContext diagnosticContext)
        {
            // save original instance of Response.Body  
            var originalBody = context.Response.Body;

            using var memory = new MemoryStream();
            context.Response.Body = memory;

            await next(context);

            memory.Seek(0, SeekOrigin.Begin);
            context.Response.Body = originalBody;
            await memory.CopyToAsync(context.Response.Body);

            var contentType = context.Response.ContentType;
            var expected = context.Request.Path.HasValue && !context.Request.Path.Value.Contains("/swagger");
            if (expected && contentType is not null && contentType.StartsWith("application/json"))
            {
                memory.Seek(0, SeekOrigin.Begin);

                var tempBody = JsonSerializer.Deserialize<object>(new StreamReader(memory).ReadToEnd());
                var formattedResponseBody = JsonSerializer.Serialize(tempBody, new JsonSerializerOptions { WriteIndented = false });
                diagnosticContext.Set("MimeType", contentType);
                diagnosticContext.Set("Data", formattedResponseBody);
            }
            else if (contentType is not null || !string.IsNullOrWhiteSpace(contentType))
            {
                diagnosticContext.Set("MimeType", contentType);
            }
        }
    }
}
