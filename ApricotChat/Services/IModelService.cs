using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApricotChat.Models;

namespace ApricotChat.Services
{
    public interface IModelService
    {
        Task<string> CompleteAsync(IEnumerable<ChatMessage> history, string userMessage, string model, CancellationToken cancellationToken = default);
    }
}
