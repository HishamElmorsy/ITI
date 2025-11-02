using System.Collections.Generic;
using System.Threading.Tasks;
using ApricotChat.Models;

namespace ApricotChat.Services
{
    public interface IGroqService
    {
        Task<string> GetChatCompletionAsync(IEnumerable<ChatMessage> history, string userMessage);
        IAsyncEnumerable<string> StreamChatAsync(IEnumerable<ChatMessage> history, string userMessage, string model);
    }
}
