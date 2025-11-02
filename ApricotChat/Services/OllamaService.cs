using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ApricotChat.Models;

namespace ApricotChat.Services
{
    public class OllamaService : IModelService
    {
        private readonly HttpClient _http;
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public OllamaService(HttpClient http)
        {
            _http = http;
        }

        public async Task<string> CompleteAsync(IEnumerable<ChatMessage> history, string userMessage, string model, CancellationToken cancellationToken = default)
        {
            var promptBuilder = new StringBuilder();
            foreach (var m in history)
            {
                promptBuilder.AppendLine($"{m.Role}: {m.Content}");
            }
            promptBuilder.AppendLine($"user: {userMessage}");

            var payload = new
            {
                model = string.IsNullOrWhiteSpace(model) ? "mistral" : model,
                prompt = promptBuilder.ToString(),
                stream = false
            };

            using var req = new HttpRequestMessage(HttpMethod.Post, "http://localhost:11434/api/generate");
            req.Content = new StringContent(JsonSerializer.Serialize(payload, JsonOptions), Encoding.UTF8, "application/json");
            using var resp = await _http.SendAsync(req, cancellationToken);
            resp.EnsureSuccessStatusCode();
            var json = await resp.Content.ReadAsStringAsync(cancellationToken);
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("response", out var response))
            {
                return response.GetString() ?? string.Empty;
            }
            return json; // fallback raw
        }
    }
}
