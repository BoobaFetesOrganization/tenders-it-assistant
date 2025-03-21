using Serilog;
using Serilog.Events;
using TendersITAssistant.Application;
using TendersITAssistant.Infrastructure;
using TendersITAssistant.Infrastructure.Api.Gemini;
using TendersITAssistant.Infrastructure.Api.Gemini.Service;
using TendersITAssistant.Infrastructure.Database.TableStorage;
using TendersITAssistant.Presentation.API;
using TendersITAssistant.Presentation.API.Configuation;

// The initial "bootstrap" logger is able to log errors during start-up. It's completely replaced by the
// logger configured in `AddSerilog()` below, once configuration and dependency-injection have both been
// set up successfully.
var logger = ConfigureSerilogService.GetBootstrapLogger();

Log.Information("Application starts up");

try
{
    // create application builder
    var builder = WebApplication.CreateBuilder(args);

    // create logger
    builder.Services.AddSerilog(builder.Configuration, logger);

    // handle the user secrets for local development only, see 'scripts/set-dev-secrets.ps1'
    builder.Configuration.AddUserSecrets<Program>();

    // Add services to the container.
    builder.Services.AddApplication(logger);
    builder.Services.AddInfrastructure(logger);
    builder.Services.AddInfrastructureDatabase(builder.Configuration, logger);
    builder.Services.AddInfrastructureApiGemini(builder.Configuration, addHttpClientCb: () =>
    {
        // services configuration
        builder.Services.AddHttpClient<GeminiGenerateContentService>();
        builder.Services.AddHttpClient<GeminiFileService>();
    }, logger);
    builder.Services.AddPresentationApi(builder.Configuration, logger);


    // Add services to the container
    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // enable middleware to log HTTP requests smartly
    app.UseSerilog(logger);

    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "swagger"; // Serve Swagger UI at the app's root
    });

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    var corsConfig = builder.Configuration.GetSection("Cors:SpaCors").Get<CorsConfiguration>()
        ?? throw new InvalidOperationException("Cors section is missing or invalid in appsettings.json, it should be {\r\n  \"Cors\": {\r\n    \"SpaCors\": {\r\n      \"Name\": \"SpaCors\",\r\n      \"Origins\": [ \"http://localhost:3000\", \"https://localhost:3000\" ],\r\n      \"AllowedVerbs\": [ \"GET\", \"POST\", \"PUT\", \"DELETE\", \"OPTIONS\" ],\r\n      \"AllowedHeaders\": [ \"*\" ]\r\n    }\r\n  }\r\n}");


    // Configurer le middleware pour servir la SPA
    app.UseDefaultFiles();
    app.UseStaticFiles();

    app.UseRouting();
    app.UsePresentationApi();
    app.UseAuthorization();
    app.MapControllers();

    // Serve the SPA for the root URL
    app.MapFallbackToFile("/");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    throw;
}
finally
{
    Log.Information("Application ends");
    Log.CloseAndFlush();
}
