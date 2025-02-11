using GenAIChat.Application;
using GenAIChat.Infrastructure;
using GenAIChat.Infrastructure.Api.Gemini;
using GenAIChat.Infrastructure.Api.Gemini.Service;
using GenAIChat.Infrastructure.Database.Sqlite;
using GenAIChat.Presentation.API;
using GenAIChat.Presentation.API.Configuation;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGenAiChatPresentationApi(builder.Configuration);
builder.Services.AddGenAiChatApplication();
builder.Services.AddGenAiChatInfrastructure(builder.Configuration);
builder.Services.AddGenAiChatInfrastructureDatabaseSqlLite(builder.Configuration);
builder.Services.AddGenAiChatInfrastructureApiGemini(builder.Configuration, addHttpClientCb: () =>
{
    // services configuration
    builder.Services.AddHttpClient<GeminiGenerateContentService>();
    builder.Services.AddHttpClient<GeminiFileService>();
});

// Add services to the container
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
app.UseGenAiChatPresentationApi();
app.UseAuthorization();
app.MapControllers();

// Serve the SPA for the root URL
app.MapFallbackToFile("/");

app.Run();
