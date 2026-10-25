using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace OllamRagPOC.Services
{
    public class QdrantService : IQdrantService
    {
        private readonly QdrantClient _client;

        public QdrantService(IConfiguration configuration)
        {
            var host = configuration["Qdrant:Host"]
                ?? throw new InvalidOperationException("Qdrant:Host configuration is required.");
            var port = configuration.GetValue<int?>("Qdrant:GrpcPort")
                ?? throw new InvalidOperationException("Qdrant:GrpcPort configuration is required.");

            _client = new QdrantClient(
                host: host,
                port: port);
        }

        public async Task CreateCollection()
        {
            await _client.CreateCollectionAsync(
                "documents",
                new VectorParams
                {
                    Size = 768,
                    Distance = Distance.Cosine
                });
        }

        public async Task StoreChunk(
            string content,
            float[] embedding)
        {
            await _client.UpsertAsync(
                "documents",
                new[]
                {
                new PointStruct
                {
                    Id = Guid.NewGuid(),
                    Vectors = embedding,
                    Payload =
                    {
                        ["content"] = content
                    }
                }
                });
        }

        public async Task<List<string>> Search(
            float[] embedding)
        {
            var result =
                await _client.QueryAsync(
                "documents",
                embedding,
                limit: 5);

            return result
                .Select(x =>
                    x.Payload["content"].StringValue)
                .ToList();
        }
    }
}
