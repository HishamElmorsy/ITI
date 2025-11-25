using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AiFileAnalyzerMvc.Models;

namespace AiFileAnalyzerMvc.Services
{
    public class OpenAIService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public OpenAIService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _apiKey = config["OpenAI:ApiKey"];
        }

        // ---- Get Embeddings (batched) ----
        private async Task<List<double[]>> GetEmbeddingsAsync(IEnumerable<string> texts)
        {
            var reqBody = new
            {
                model = "text-embedding-3-large", // higher dimensionality, better accuracy
                input = texts.ToArray()
            };

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);

            const int maxRetries = 3;
            int baseDelayMs = 1000;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                // HttpContent is single-use, so recreate per attempt
                var content = new StringContent(JsonSerializer.Serialize(reqBody), Encoding.UTF8, "application/json");
                var res = await _http.PostAsync("https://api.openai.com/v1/embeddings", content);
                var json = await res.Content.ReadAsStringAsync();

                if (res.IsSuccessStatusCode)
                {
                    using var doc = JsonDocument.Parse(json);
                    if (!doc.RootElement.TryGetProperty("data", out var data))
                    {
                        // Unexpected but non-error status — log and throw
                        Console.WriteLine("[OpenAIService] Embeddings response missing 'data' property. Response: " + json);
                        throw new Exception("Embeddings response missing 'data' property from OpenAI.");
                    }

                    var list = new List<double[]>();
                    foreach (var item in data.EnumerateArray())
                    {
                        if (item.TryGetProperty("embedding", out var embeddingElem))
                        {
                            var embedding = embeddingElem
                                .EnumerateArray()
                                .Select(e => e.GetDouble())
                                .ToArray();
                            list.Add(Normalize(embedding));
                        }
                    }

                    return list;
                }

                // Non-success: try to parse an error message
                string serverErrorMessage = null;
                try
                {
                    using var errDoc = JsonDocument.Parse(json);
                    if (errDoc.RootElement.TryGetProperty("error", out var err) &&
                        err.TryGetProperty("message", out var msgProp))
                    {
                        serverErrorMessage = msgProp.GetString();
                    }
                }
                catch
                {
                    // ignore parse errors
                }

                int status = (int)res.StatusCode;
                bool isTransient = status == 429 || (status >= 500 && status < 600);

                Console.WriteLine($"[OpenAIService] Attempt {attempt}/{maxRetries} - HTTP {status}. Transient={isTransient}. Message='{serverErrorMessage ?? json}'");

                if (!isTransient || attempt == maxRetries)
                {
                    // Final failure — surface a clear error
                    if (!string.IsNullOrEmpty(serverErrorMessage))
                        throw new Exception($"OpenAI API error {status}: {serverErrorMessage}");
                    else
                        throw new Exception($"OpenAI embeddings request failed with status {status}: {json}");
                }

                // Respect Retry-After header if present
                int delayMs = baseDelayMs * attempt;
                if (res.Headers.TryGetValues("Retry-After", out var vals) && int.TryParse(vals.FirstOrDefault(), out var ra))
                {
                    delayMs = ra * 1000;
                }

                await Task.Delay(delayMs);
            }

            throw new Exception("Failed to fetch embeddings from OpenAI after retries.");
        }

        // ---- Normalize Vector ----
        private double[] Normalize(double[] vector)
        {
            double mag = Math.Sqrt(vector.Sum(v => v * v));
            return vector.Select(v => v / mag).ToArray();
        }

        // ---- Cosine Similarity ----
        private double CosineSimilarity(double[] a, double[] b)
        {
            if (a.Length == 0 || b.Length == 0) return 0;
            return a.Zip(b, (x, y) => x * y).Sum();
        }

        // ---- Semantic Chunking ----
        private List<string> SemanticChunkText(string text, int targetChunkSize = 1000)
        {
            var sentences = text.Split(new[] { '.', '?', '!' }, StringSplitOptions.RemoveEmptyEntries);
            var chunks = new List<string>();
            var current = new StringBuilder();

            foreach (var s in sentences)
            {
                var sentence = s.Trim();
                if (sentence.Length == 0) continue;

                if ((current.Length + sentence.Length) > targetChunkSize)
                {
                    chunks.Add(current.ToString().Trim());
                    current.Clear();
                }

                current.Append(sentence + ". ");
            }

            if (current.Length > 0)
                chunks.Add(current.ToString().Trim());

            return chunks;
        }

        // ---- Main Ask Method ----
        public async Task<OpenAIResponse> AskQuestionAsync(string fileText, string question)
        {
            // Step 1️⃣: Split file text into semantic chunks
            var chunks = SemanticChunkText(fileText);
            if (chunks.Count == 0)
            {
                return new OpenAIResponse
                {
                    Answer = "No readable text could be extracted from the uploaded file.",
                    Similarity = 0
                };
            }

            // Step 2️⃣: Compute embeddings for all chunks + question (in one request)
            var allTexts = new List<string>(chunks) { question };
            var embeddings = await GetEmbeddingsAsync(allTexts);
            var questionEmbedding = embeddings.Last();
            var chunkEmbeddings = embeddings.Take(chunks.Count).ToList();

            // Step 3️⃣: Find the most semantically similar chunk
            double bestSim = -1;
            string bestChunk = "";
            for (int i = 0; i < chunks.Count; i++)
            {
                double sim = CosineSimilarity(chunkEmbeddings[i], questionEmbedding);
                if (sim > bestSim)
                {
                    bestSim = sim;
                    bestChunk = chunks[i];
                }
            }

            double similarityScore = Math.Round(bestSim * 100, 2);
            Console.WriteLine($"[DEBUG] Highest Similarity: {similarityScore}%");
            Console.WriteLine($"[DEBUG] Best Chunk: {bestChunk.Substring(0, Math.Min(bestChunk.Length, 150))}...");

            // Step 4️⃣: Always use best chunk as source of truth (no rejection)
            var prompt = @$"
You are an intelligent file analysis assistant.
Answer the question **only** using the information in the following text section.
If the answer cannot be found there, say: 'The uploaded file does not contain information about that.'

Relevant section from file:
{bestChunk}

User question:
{question}
";

            // Step 5️⃣: Query GPT
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);

            var chatReq = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "system", content = "You are a factual assistant that analyzes text context." },
                    new { role = "user", content = prompt }
                }
            };

            var chatContent = new StringContent(JsonSerializer.Serialize(chatReq), Encoding.UTF8, "application/json");
            var chatRes = await _http.PostAsync("https://api.openai.com/v1/chat/completions", chatContent);
            var chatJson = await chatRes.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(chatJson);
            string answer = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            // Step 6️⃣: Return
            return new OpenAIResponse
            {
                Answer = answer,
                Similarity = similarityScore
            };
        }
    }
}