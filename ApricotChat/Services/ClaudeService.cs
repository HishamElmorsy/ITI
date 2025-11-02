using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApricotChat.Models;

namespace ApricotChat.Services
{
    public class ClaudeService : IModelService
    {
        public Task<string> CompleteAsync(IEnumerable<ChatMessage> history, string userMessage, string model, CancellationToken cancellationToken = default)
        {
            // Placeholder: integrate Anthropic Claude API here
            return Task.FromResult("[Claude stub] Configure API key and implementation to enable this model.");
        }
    }
}
