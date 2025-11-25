using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AiFileAnalyzerMvc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System.Reflection.PortableExecutable;

namespace AiFileAnalyzerMvc.Services
{
    public class FileProcessingService
    {
        private readonly IWebHostEnvironment _env;

        public FileProcessingService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<FileData> SaveAndExtractAsync(IFormFile file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            string folderPath = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads");
            Directory.CreateDirectory(folderPath);

            // keep unique name to avoid clashes
            var safeName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            string filePath = Path.Combine(folderPath, safeName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string text = ExtractText(filePath);

            return new FileData
            {
                FileName = file.FileName ?? safeName,
                FilePath = filePath,
                ExtractedText = text
            };
        }

        private string ExtractText(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLowerInvariant();

            if (ext == ".txt")
            {
                return File.ReadAllText(filePath);
            }

            if (ext == ".pdf")
            {
                var sb = new StringBuilder();
                using var reader = new PdfReader(filePath);
                using var pdf = new PdfDocument(reader);
                int pages = pdf.GetNumberOfPages();
                for (int i = 1; i <= pages; i++)
                {
                    var page = pdf.GetPage(i);
                    var pageText = PdfTextExtractor.GetTextFromPage(page);
                    sb.AppendLine(pageText);
                }
                return sb.ToString();
            }

            if (ext == ".docx")
            {
                using var word = WordprocessingDocument.Open(filePath, false);
                var body = word.MainDocumentPart?.Document?.Body;
                return body?.InnerText ?? string.Empty;
            }

            return string.Empty; // unsupported type -> empty string
        }
    }
}
