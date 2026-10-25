using OllamRagPOC.Services;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var ollamaBaseUrl = builder.Configuration["Ollama:BaseUrl"]
    ?? throw new InvalidOperationException("Ollama:BaseUrl configuration is required.");

builder.Services.AddHttpClient<IOllamaEmbeddingService, OllamaEmbeddingService>(
    client => client.BaseAddress = new Uri(ollamaBaseUrl));

builder.Services.AddHttpClient<IOllamaChatService, OllamaChatService>(
    client => client.BaseAddress = new Uri(ollamaBaseUrl));

builder.Services.AddSingleton<IQdrantService, QdrantService>();
builder.Services.AddSingleton<IPdfService, PdfService>();
builder.Services.AddSingleton<IChunkingService, ChunkingService>();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }
    catch (RpcException exception) when (exception.StatusCode == StatusCode.Unavailable)
    {
        context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status503ServiceUnavailable,
            Title = "Qdrant is unavailable.",
            Detail = "Start Qdrant and verify the configured gRPC endpoint is reachable."
        });
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
