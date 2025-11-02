using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApricotChat.Models;

namespace ApricotChat.Services
{
    public class ModelRouter : IModelRouter
    {
        private readonly IModelService _groq;
        private readonly IModelService _ollama;
        private readonly IModelService _gemini;
        private readonly IModelService _claude;
        private readonly IModelService _meta;
        private readonly IModelService _deepseek;

        public ModelRouter(
            GroqService groq,
            OllamaService ollama,
            GeminiService gemini,
            ClaudeService claude,
            MetaLlamaService meta,
            DeepSeekService deepseek)
        {
            _groq = groq;
            _ollama = ollama;
            _gemini = gemini;
            _claude = claude;
            _meta = meta;
            _deepseek = deepseek;
        }

        public Task<string> CompleteAsync(string provider, string model, IEnumerable<ChatMessage> history, string userMessage, CancellationToken cancellationToken = default)
        {
            provider = (provider ?? "groq").ToLowerInvariant();
            return provider switch
            {
                "groq" => _groq.CompleteAsync(history, userMessage, model, cancellationToken),
                "ollama" => _ollama.CompleteAsync(history, userMessage, model, cancellationToken),
                "google" => _gemini.CompleteAsync(history, userMessage, model, cancellationToken),
                "anthropic" => _claude.CompleteAsync(history, userMessage, model, cancellationToken),
                "meta" => _meta.CompleteAsync(history, userMessage, model, cancellationToken),
                "deepseek" => _deepseek.CompleteAsync(history, userMessage, model, cancellationToken),
                _ => _groq.CompleteAsync(history, userMessage, model, cancellationToken)
            };
        }
    }
}
