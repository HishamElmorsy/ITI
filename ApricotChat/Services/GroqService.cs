using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using ApricotChat.Models;
using System.IO;

namespace ApricotChat.Services
{
    public class GroqService : IGroqService, IModelService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public GroqService(HttpClient http, IOptions<GroqOptions> options)
        {
            _http = http;
            _apiKey = options.Value.ApiKey;
        }

        public async Task<string> GetChatCompletionAsync(IEnumerable<ChatMessage> history, string userMessage)
        {
            return await CompleteAsync(history, userMessage, "llama-3.1-8b-instant");
        }

        public async Task<string> CompleteAsync(IEnumerable<ChatMessage> history, string userMessage, string model, System.Threading.CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                throw new HttpRequestException("Groq API key is not configured. Set 'Groq:ApiKey' via user-secrets.");
            }

            var messages = new List<object>();
            foreach (var m in history)
            {
                messages.Add(new { role = m.Role, content = m.Content });
            }
            messages.Add(new { role = "user", content = userMessage });

            using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.groq.com/openai/v1/chat/completions");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            req.Headers.Accept.Clear();
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
            req.Headers.Accept.Clear();
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
            var body = new
            {
                model = string.IsNullOrWhiteSpace(model) ? "llama-3.1-8b-instant" : model,
                messages = messages
            };
            req.Content = new StringContent(JsonSerializer.Serialize(body, JsonOptions), Encoding.UTF8, "application/json");

            using var resp = await _http.SendAsync(req, cancellationToken);
            if (!resp.IsSuccessStatusCode)
            {
                var errorBody = await resp.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException($"Groq API request failed with status {(int)resp.StatusCode} {resp.ReasonPhrase}: {errorBody}");
            }
            using var stream = await resp.Content.ReadAsStreamAsync(cancellationToken);
            using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
            var root = doc.RootElement;
            var content = root.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
            return content ?? string.Empty;
        }

        public async IAsyncEnumerable<string> StreamChatAsync(IEnumerable<ChatMessage> history, string userMessage, string model)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                throw new HttpRequestException("Groq API key is not configured. Set 'Groq:ApiKey' via user-secrets.");
            }

            var messages = new List<object>();
            foreach (var m in history)
            {
                messages.Add(new { role = m.Role, content = m.Content });
            }
            messages.Add(new { role = "user", content = userMessage });

            using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.groq.com/openai/v1/chat/completions");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            var body = new
            {
                model = string.IsNullOrWhiteSpace(model) ? "llama-3.1-8b-instant" : model,
                messages = messages,
                stream = true
            };
            req.Content = new StringContent(JsonSerializer.Serialize(body, JsonOptions), Encoding.UTF8, "application/json");

            using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead);
            if (!resp.IsSuccessStatusCode)
            {
                var errorBody = await resp.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Groq API request failed with status {(int)resp.StatusCode} {resp.ReasonPhrase}: {errorBody}");
            }

            await using var stream = await resp.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (!line.StartsWith("data:")) continue;
                var payload = line.Substring("data:".Length).Trim();
                if (payload == "[DONE]") yield break;
                string? pieceToYield = null;
                // Parse payload safely without yielding inside try/catch
                try
                {
                    using var doc = JsonDocument.Parse(payload);
                    var root = doc.RootElement;
                    if (root.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
                    {
                        var choice = choices[0];
                        if (choice.TryGetProperty("delta", out var delta) && delta.TryGetProperty("content", out var contentEl))
                        {
                            pieceToYield = contentEl.GetString();
                        }
                    }
                }
                catch { /* ignore malformed lines */ }

                if (!string.IsNullOrEmpty(pieceToYield))
                {
                    yield return pieceToYield;
                }
            }
        }
    }
}
