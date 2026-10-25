using Microsoft.AspNetCore.Mvc;
using OllamRagPOC.Models;
using OllamRagPOC.Services;

namespace OllamRagPOC.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Ask(
            AskRequest request,
            IOllamaEmbeddingService embedding,
            IQdrantService qdrant,
            IOllamaChatService chat)
        {
            var questionEmbedding =
                await embedding.EmbedAsync(
                    request.Question);

            var chunks =
                await qdrant.Search(
                    questionEmbedding);

            var context =
                string.Join("\n\n", chunks);

            var answer =
                await chat.AskAsync(
                    request.Question,
                    context);

            return Ok(answer);
        }
    }
}