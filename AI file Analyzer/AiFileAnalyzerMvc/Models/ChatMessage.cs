namespace AiFileAnalyzerMvc.Models
{
    public class ChatMessage
    {
        public string Role { get; set; } = string.Empty; // "user" / "assistant"
        public string Content { get; set; } = string.Empty;
    }
}
