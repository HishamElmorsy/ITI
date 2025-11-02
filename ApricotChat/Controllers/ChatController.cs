using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApricotChat.Data;
using ApricotChat.Models;
using ApricotChat.Services;
using Microsoft.AspNetCore.SignalR;
using ApricotChat.Hubs;

namespace ApricotChat.Controllers
{
    public class ChatController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IGroqService _groq;
        private readonly IModelRouter _router;
        private readonly IHubContext<ChatHub> _hub;

        public ChatController(AppDbContext db, IGroqService groq, IModelRouter router, IHubContext<ChatHub> hub)
        {
            _db = db;
            _groq = groq;
            _router = router;
            _hub = hub;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? id, string? modelKey)
        {
            ChatSession session = new ChatSession();
            if (id.HasValue)
            {
                session = await _db.ChatSessions.Include(s => s.Messages)
                    .OrderBy(s => s.Id)
                    .FirstOrDefaultAsync(s => s.Id == id.Value) ?? new ChatSession();
            }

            var recent = await _db.ChatSessions
                .OrderByDescending(s => s.Id)
                .Take(20)
                .ToListAsync();

            var vm = new ChatViewModel
            {
                Session = session,
                RecentSessions = recent
            };

            vm.ModelOptions = new()
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem{ Value = "groq:llama-3.1-8b-instant", Text = "Groq • llama-3.1-8b-instant" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem{ Value = "google:gemini-pro", Text = "Google • gemini-pro" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem{ Value = "anthropic:claude-3-sonnet", Text = "Anthropic • claude-3-sonnet" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem{ Value = "meta:llama-3.1-8b-instant", Text = "Meta • llama-3.1-8b" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem{ Value = "deepseek:deepseek-chat", Text = "DeepSeek • deepseek-chat" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem{ Value = "ollama:mistral", Text = "Ollama • mistral (local)" }
            };
            vm.SelectedModelKey = string.IsNullOrWhiteSpace(modelKey) ? vm.SelectedModelKey : modelKey;

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> New(string? modelKey)
        {
            var session = new ChatSession();
            _db.ChatSessions.Add(session);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = session.Id, modelKey });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendStream(int id, string message, string? modelKey)
        {
            var session = await _db.ChatSessions.Include(s => s.Messages).FirstOrDefaultAsync(s => s.Id == id);
            if (session == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                return Ok();
            }

            // Save user message first
            var userMsg = new ChatMessage { ChatSessionId = session.Id, Role = "user", Content = message };
            _db.ChatMessages.Add(userMsg);
            await _db.SaveChangesAsync();

            // Parse provider:model from modelKey
            var provider = "groq";
            var model = "llama-3.1-8b-instant";
            if (!string.IsNullOrWhiteSpace(modelKey) && modelKey.Contains(':'))
            {
                var parts = modelKey.Split(':');
                if (parts.Length >= 2)
                {
                    provider = parts[0];
                    model = parts[1];
                }
            }

            string reply;
            // Provider-aware streaming: for Groq use true streaming, else fallback to full then simulate
            if (provider.Equals("groq", System.StringComparison.OrdinalIgnoreCase))
            {
                // notify client to prepare streaming UI immediately
                await _hub.Clients.Group(session.Id.ToString()).SendAsync("started");
                var assembled = new System.Text.StringBuilder();
                await foreach (var piece in _groq.StreamChatAsync(session.Messages.OrderBy(m => m.Id), message, model))
                {
                    assembled.Append(piece);
                    await _hub.Clients.Group(session.Id.ToString()).SendAsync("chunk", assembled.ToString());
                }
                await _hub.Clients.Group(session.Id.ToString()).SendAsync("completed");
                reply = assembled.ToString();
            }
            else
            {
                // Get full reply (fallback) and simulate typing
                reply = await _router.CompleteAsync(provider, model, session.Messages.OrderBy(m => m.Id), message);
                var words = reply.Split(' ');
                var assembled = new System.Text.StringBuilder();
                foreach (var w in words)
                {
                    if (assembled.Length > 0) assembled.Append(' ');
                    assembled.Append(w);
                    await _hub.Clients.Group(session.Id.ToString()).SendAsync("chunk", assembled.ToString());
                    await Task.Delay(75);
                }
                await _hub.Clients.Group(session.Id.ToString()).SendAsync("completed");
            }

            // Save assistant message
            var aiMsg = new ChatMessage { ChatSessionId = session.Id, Role = "assistant", Content = reply };
            _db.ChatMessages.Add(aiMsg);
            await _db.SaveChangesAsync();

            // If this was an AJAX request, stay on the page; otherwise redirect back to the session
            var isAjax = string.Equals(Request.Headers["X-Requested-With"], "XMLHttpRequest", System.StringComparison.OrdinalIgnoreCase);
            if (isAjax)
            {
                return Ok();
            }
            return RedirectToAction(nameof(Index), new { id = session.Id, modelKey });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(int id, string message, string? modelKey)
        {
            var session = await _db.ChatSessions.Include(s => s.Messages).FirstOrDefaultAsync(s => s.Id == id);
            if (session == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(message))
            {
                var userMsg = new ChatMessage { ChatSessionId = session.Id, Role = "user", Content = message };
                _db.ChatMessages.Add(userMsg);
                await _db.SaveChangesAsync();

                // Parse provider:model from modelKey
                var provider = "groq";
                var model = "llama-3.1-8b-instant";
                if (!string.IsNullOrWhiteSpace(modelKey) && modelKey.Contains(':'))
                {
                    var parts = modelKey.Split(':');
                    if (parts.Length >= 2)
                    {
                        provider = parts[0];
                        model = parts[1];
                    }
                }

                var reply = await _router.CompleteAsync(provider, model, session.Messages.OrderBy(m => m.Id), message);
                var aiMsg = new ChatMessage { ChatSessionId = session.Id, Role = "assistant", Content = reply };
                _db.ChatMessages.Add(aiMsg);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { id = session.Id, modelKey });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var session = await _db.ChatSessions.Include(s => s.Messages).FirstOrDefaultAsync(s => s.Id == id);
            if (session == null)
            {
                return NotFound();
            }

            _db.ChatSessions.Remove(session);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
