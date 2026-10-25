namespace OllamRagPOC.Services
{
    public interface IOllamaChatService
    {
        Task<string> AskAsync(string question, string context);
    }
}
