using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApricotChat.Models;

namespace ApricotChat.Services
{
    public class MetaLlamaService : IModelService
    {
        public Task<string> CompleteAsync(IEnumerable<ChatMessage> history, string userMessage, string model, CancellationToken cancellationToken = default)
        {
            // Placeholder: integrate Meta LLaMA via provider (hosted) if available in future
            return Task.FromResult("[Meta LLaMA stub] Connect to a hosted API to enable this model.");
        }
    }
}
