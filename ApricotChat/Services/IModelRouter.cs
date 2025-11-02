using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApricotChat.Models;

namespace ApricotChat.Services
{
    public interface IModelRouter
    {
        Task<string> CompleteAsync(string provider, string model, IEnumerable<ChatMessage> history, string userMessage, CancellationToken cancellationToken = default);
    }
}
