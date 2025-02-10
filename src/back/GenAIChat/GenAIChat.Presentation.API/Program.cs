using GenAIChat.Presentation.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGenAiChatServices(builder.Configuration);

// Add services to the container
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

app.UseGenAiChatPresentationServices(builder.Configuration);

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

// Serve the SPA for the root URL
app.MapFallbackToFile("/index.html");



app.Run();
