namespace AiFileAnalyzerMvc.Models
{
    public class OpenAIResponse
    {
        public string Answer { get; set; } = string.Empty;
        public double Similarity { get; set; }
    }
}
