namespace OllamRagPOC.Services
{
    public interface IQdrantService
    {
        Task CreateCollection();

        Task StoreChunk(string content, float[] embedding);

        Task<List<string>> Search(float[] embedding);
    }
}
