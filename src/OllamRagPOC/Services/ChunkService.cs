namespace OllamRagPOC.Services
{
    public class ChunkingService : IChunkingService
    {
        public List<string> Chunk(string text,
            int size = 500,
            int overlap = 50)
        {
            var chunks = new List<string>();

            for (int i = 0; i < text.Length; i += size - overlap)
            {
                chunks.Add(text.Substring(
                    i,
                    Math.Min(size, text.Length - i)));
            }

            return chunks;
        }
    }
}
