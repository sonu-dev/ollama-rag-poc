using Microsoft.AspNetCore.Mvc;
using OllamRagPOC.Services;

namespace OllamRagPOC.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class UploadController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Upload(
            IFormFile file,
            IPdfService pdfService,
            IChunkingService chunking,
            IOllamaEmbeddingService embedding,
            IQdrantService qdrant)
        {
            var path =
                Path.Combine(
                    Path.GetTempPath(),
                    file.FileName);

            using (var stream = System.IO.File.Create(path))
            {
                await file.CopyToAsync(stream);
            }

            var text =
                pdfService.ExtractText(path);

            var chunks =
                chunking.Chunk(text);

            foreach (var chunk in chunks)
            {
                var vector =
                    await embedding
                        .EmbedAsync(chunk);

                await qdrant.StoreChunk(
                    chunk,
                    vector);
            }

            return Ok("Document Indexed");
        }
    }
}
