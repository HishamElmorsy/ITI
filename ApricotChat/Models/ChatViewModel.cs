using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ApricotChat.Models
{
    public class ChatViewModel
    {
        public ChatSession Session { get; set; } = new ChatSession();
        public List<ChatSession> RecentSessions { get; set; } = new List<ChatSession>();
        public string SelectedModelKey { get; set; } = "groq:llama-3.1-8b-instant";
        public List<SelectListItem> ModelOptions { get; set; } = new();
    }
}
