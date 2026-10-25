namespace OllamRagPOC.Services
{
    public class OllamaChatService : IOllamaChatService
    {
        private readonly HttpClient _client;

        public OllamaChatService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> AskAsync(
            string question,
            string context)
        {
            var prompt = $"""

        Answer using only the provided context.

        Context:
        {context}

        Question:
        {question}

        """;

            var response =
                await _client.PostAsJsonAsync(
                    "/api/generate",
                    new
                    {
                        model = "qwen2.5:7b",
                        prompt,
                        stream = false
                    });

            var result =
                await response.Content
                .ReadFromJsonAsync<ChatResponse>();

            return result!.Response;
        }
    }

    public class ChatResponse
    {
        public string Response { get; set; } = "";
    }
}
