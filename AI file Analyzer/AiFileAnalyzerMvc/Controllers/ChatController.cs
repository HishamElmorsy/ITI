using Microsoft.AspNetCore.Mvc;
using AiFileAnalyzerMvc.Services;
using AiFileAnalyzerMvc.Models;
using System.Collections.Generic;
using System.Linq;

namespace AiFileAnalyzerMvc.Controllers
{
    public class ChatController : Controller
    {
        private readonly FileProcessingService _fileService;
        private readonly OpenAIService _aiService;
        private static FileData _uploadedFile;
        private static List<FileData> _uploadedFiles = new List<FileData>();

        public ChatController(FileProcessingService fileService, OpenAIService aiService)
        {
            _fileService = fileService;
            _aiService = aiService;
        }

        public IActionResult Index()
        {
            ViewBag.UploadedFiles = _uploadedFiles;
            ViewBag.CurrentFile = _uploadedFile;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            _uploadedFile = await _fileService.SaveAndExtractAsync(file);
            
            // Add to list if not already present
            if (!_uploadedFiles.Any(f => f.FileName == _uploadedFile.FileName))
            {
                _uploadedFiles.Add(_uploadedFile);
            }
            
            ViewBag.Message = $"✅ File '{file.FileName}' uploaded successfully! You can now ask questions about it.";
            ViewBag.UploadedFiles = _uploadedFiles;
            ViewBag.CurrentFile = _uploadedFile;
            return View("Index");
        }

        [HttpPost]
        public IActionResult SelectFile(string fileName)
        {
            _uploadedFile = _uploadedFiles.FirstOrDefault(f => f.FileName == fileName);
            ViewBag.UploadedFiles = _uploadedFiles;
            ViewBag.CurrentFile = _uploadedFile;
            ViewBag.Message = $"📄 Now using file: {fileName}";
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Ask(string question)
        {
            if (_uploadedFile == null)
                return Json(new { answer = "⚠️ Please upload a file first." });

            try
            {
                var result = await _aiService.AskQuestionAsync(_uploadedFile.ExtractedText, question);
                if (result == null)
                    return Json(new { answer = "⚠️ No response from AI service." });

                // Return consistent shape expected by the front-end
                return Json(new { answer = result.Answer, similarity = result.Similarity });
            }
            catch (Exception ex)
            {
                // Log server-side for diagnostics
                Console.WriteLine($"[ChatController] Ask error: {ex}");

                // Provide a clear, user-friendly message (don't leak sensitive internals)
                string userMessage;
                if (ex.Message?.Contains("quota", StringComparison.OrdinalIgnoreCase) == true)
                {
                    userMessage = "⚠️ OpenAI quota exceeded. Check your OpenAI plan/billing dashboard.";
                }
                else
                {
                    userMessage = "⚠️ OpenAI request failed. Try again later.";
                }

                return Json(new { answer = userMessage });
            }
        }
    }
}
