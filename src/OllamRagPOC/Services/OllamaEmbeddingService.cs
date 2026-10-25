namespace OllamRagPOC.Services
{
    public class OllamaEmbeddingService : IOllamaEmbeddingService
    {
        private readonly HttpClient _client;

        public OllamaEmbeddingService(HttpClient client)
        {
            _client = client;
        }

        public async Task<float[]> EmbedAsync(string text)
        {
            var response = await _client.PostAsJsonAsync(
                "/api/embeddings",
                new
                {
                    model = "nomic-embed-text",
                    prompt = text
                });

            response.EnsureSuccessStatusCode();

            var result =
                await response.Content
                .ReadFromJsonAsync<EmbeddingResult>();

            return result!.Embedding;
        }
    }

    public class EmbeddingResult
    {
        public float[] Embedding { get; set; } = [];
    }
}
