namespace OllamRagPOC.Services
{
    public interface IOllamaEmbeddingService
    {
        Task<float[]> EmbedAsync(string text);
    }
}
