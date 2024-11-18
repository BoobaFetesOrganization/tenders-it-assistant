using GenAIChat.Presentation.API;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGenAiChatServices(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

// add a physical directory for uploaded files (by project)
var rootDir = Path.Combine(Path.GetDirectoryName(app.Environment.ContentRootPath)!, "projects");
if (!Directory.Exists(rootDir)) Directory.CreateDirectory(rootDir);

app.Run();
