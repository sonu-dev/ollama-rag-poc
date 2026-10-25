namespace OllamRagPOC.Services
{
    public interface IChunkingService
    {
        List<string> Chunk(string text, int size = 500, int overlap = 50);
    }
}
