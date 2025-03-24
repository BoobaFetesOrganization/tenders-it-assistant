using Serilog;
using TendersITAssistant.Presentation.API.Middlewares;

namespace TendersITAssistant.Presentation.API
{
    public static class ConfigureSerilogService
    {
        public static Serilog.ILogger GetBootstrapLogger()
        {

            return new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [Start Up] {Message:lj}{NewLine}{Exception}")
                .CreateBootstrapLogger().ForContext<Program>();
        }

        public static void AddSerilog(this IServiceCollection services, IConfiguration configuration, Serilog.ILogger logger)
        {
            logger.Information("Add serilog to the services");

            services.AddSerilog((services, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(configuration));
        }

        public static void UseSerilog(this IApplicationBuilder app, Serilog.ILogger logger)
        {
            // the order of the middlewre is really important: 
            // since ResponseBodyReader write a diagnostic context value, it should be recorded after the useSerilogRequestLogging
            // to be invoked BEFORE useSerilogRequestLogging

            logger.Information("configure UseSerilogRequestLogging");
            app.UseSerilogRequestLogging(options =>
            {
                options.IncludeQueryInRequestPath = true;

                // Attach additional properties to the request completion event
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    // request metadata
                    diagnosticContext.Set("Host", httpContext.Request.Host.Value ?? "");
                    diagnosticContext.Set("Scheme", httpContext.Request.Scheme);

                    // response metadata
                    //NOTE : {Data} and {MimeType} are set by the ResponseBodyReaderMiddleware
                };
            });

            logger.Information("add ResponseBodyReaderMiddleware");
            app.UseResponseBodyReaderMiddleware();
        }
    }
}
