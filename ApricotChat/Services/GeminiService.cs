using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ApricotChat.Models;
using Microsoft.Extensions.Options;

namespace ApricotChat.Services
{
    public class GeminiOptions
    {
        public string ApiKey { get; set; } = string.Empty;
    }

    public class GeminiService : IModelService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public GeminiService(HttpClient http, IOptions<GeminiOptions> options)
        {
            _http = http;
            _apiKey = options.Value.ApiKey;
        }

        public async Task<string> CompleteAsync(IEnumerable<ChatMessage> history, string userMessage, string model, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                return "[Gemini] API key not configured. Set 'Gemini:ApiKey' via user-secrets.";
            }

            // Default to gemini-pro if unspecified
            var modelName = string.IsNullOrWhiteSpace(model) ? "gemini-pro" : model;
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{modelName}:generateContent?key={_apiKey}";

            // Map history to Gemini contents
            var contents = new List<object>();
            foreach (var m in history ?? Enumerable.Empty<ChatMessage>())
            {
                var role = m.Role == "assistant" ? "model" : "user";
                contents.Add(new
                {
                    role,
                    parts = new[] { new { text = m.Content } }
                });
            }
            contents.Add(new { role = "user", parts = new[] { new { text = userMessage } } });

            var payload = new { contents };
            using var req = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonSerializer.Serialize(payload, JsonOptions), Encoding.UTF8, "application/json")
            };

            using var resp = await _http.SendAsync(req, cancellationToken);
            if (!resp.IsSuccessStatusCode)
            {
                var err = await resp.Content.ReadAsStringAsync(cancellationToken);
                return $"[Gemini error] {(int)resp.StatusCode} {resp.ReasonPhrase}: {err}";
            }
            using var stream = await resp.Content.ReadAsStreamAsync(cancellationToken);
            using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
            var root = doc.RootElement;
            // Parse candidates[0].content.parts[0].text
            if (root.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
            {
                var cand = candidates[0];
                if (cand.TryGetProperty("content", out var content) && content.TryGetProperty("parts", out var parts) && parts.GetArrayLength() > 0)
                {
                    var part0 = parts[0];
                    if (part0.TryGetProperty("text", out var textEl))
                    {
                        return textEl.GetString() ?? string.Empty;
                    }
                }
            }
            return "[Gemini] No content returned.";
        }
    }
}
