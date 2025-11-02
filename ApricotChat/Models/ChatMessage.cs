using System;

namespace ApricotChat.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public int ChatSessionId { get; set; }
        public ChatSession? ChatSession { get; set; }
        public string Role { get; set; } = "user"; // user | assistant
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
