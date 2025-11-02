using System;
using System.Collections.Generic;

namespace ApricotChat.Models
{
    public class ChatSession
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }
}
