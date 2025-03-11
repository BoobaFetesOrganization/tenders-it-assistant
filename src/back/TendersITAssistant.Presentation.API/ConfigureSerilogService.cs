using Serilog;
using Serilog.Events;
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

            services.AddSerilog((services, loggerConfiguration) =>
            {
                loggerConfiguration
                    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
                    .ReadFrom.Configuration(configuration);
            });
        }

        public static void UseSerilog(this IApplicationBuilder app, Serilog.ILogger logger)
        {
            // the order of the middlewre is really important: 
            // since ResponseBodyReader write a diagnostic context value, it should be recorded after the useSerilogRequestLogging
            // to be invoked BEFORE useSerilogRequestLogging

            logger.Information("configure UseSerilogRequestLogging");
            app.UseSerilogRequestLogging(options =>
            {
                // Customize the message template
                options.MessageTemplate = "{RequestFullPath}\r\n > verb : {RequestMethod}\r\n > status : {StatusCode}\r\n > elapsed (ms) : {Elapsed:0.0000}\r\n > response : {ResponseBody}";

                // Attach additional properties to the request completion event
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    //NOTE : {ResponseBody} is set by the ResponseBodyReaderMiddleware
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value ?? "not set");
                    var requestFullPath = $"{httpContext.Request.Scheme}://{httpContext.Request.Path}{httpContext.Request.QueryString}";
                    diagnosticContext.Set("RequestFullPath", requestFullPath);
                };
            });

            logger.Information("add ResponseBodyReaderMiddleware");
            app.UseResponseBodyReaderMiddleware();
        }
    }
}
