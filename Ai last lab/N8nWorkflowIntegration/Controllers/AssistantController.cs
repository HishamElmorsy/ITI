using Microsoft.AspNetCore.Mvc;
using N8nWorkflowIntegration.Models;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace N8nWorkflowIntegration.Controllers;

public class AssistantController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AssistantController> _logger;
    private readonly AppSettings _appSettings;

    public AssistantController(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<AssistantController> logger,
        IOptions<AppSettings> appSettings)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
        _appSettings = appSettings.Value;
    }

    /// <summary>
    /// GET: Displays the main assistant interface
    /// </summary>
    [HttpGet]
    public IActionResult Index()
    {
        ViewData["AppName"] = _appSettings.Application?.Name ?? "AI Assistant";
        return View();
    }

    /// <summary>
    /// POST: Processes user query and returns AI response
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(string prompt, string language = "en", bool useRag = false)
    {
        // Validate input
        var maxLength = _configuration.GetValue<int>("Features:MaxPromptLength", 4000);
        
        if (string.IsNullOrWhiteSpace(prompt))
        {
            ViewBag.Error = "Please enter your question or request.";
            return View();
        }

        if (prompt.Length > maxLength)
        {
            ViewBag.Error = $"Your question is too long. Maximum length is {maxLength} characters.";
            return View();
        }

        try
        {
            // Prepare the request payload
            var requestPayload = new WorkflowRequest
            {
                UserId = User.Identity?.Name ?? "anonymous",
                Prompt = prompt,
                Metadata = new RequestMetadata
                {
                    Language = language
                }
            };

            // Get the n8n HTTP client
            var client = _httpClientFactory.CreateClient("n8n");

            // Determine which webhook path to use
            var webhookPath = useRag 
                ? _configuration["N8N:RagWebhookPath"] 
                : _configuration["N8N:WebhookPath"];

            if (string.IsNullOrEmpty(webhookPath))
            {
                throw new InvalidOperationException("Webhook path is not configured.");
            }

            // Serialize the request
            var jsonContent = JsonSerializer.Serialize(requestPayload);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            _logger.LogInformation("Sending request to n8n workflow: {WebhookPath}", webhookPath);

            // Send POST request to n8n webhook
            var response = await client.PostAsync(webhookPath, httpContent);

            // Read the response content
            var responseContent = await response.Content.ReadAsStringAsync();

            // Check if the request was successful
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("n8n request failed with status {StatusCode}: {Response}", 
                    response.StatusCode, responseContent);
                
                ViewBag.Error = "We're sorry, but we encountered an error processing your request.";
                ViewBag.ErrorDetails = responseContent;
                return View("Error");
            }

            // Deserialize the response
            var workflowResponse = JsonSerializer.Deserialize<WorkflowResponse>(responseContent);

            if (workflowResponse == null)
            {
                _logger.LogError("Failed to deserialize n8n response");
                ViewBag.Error = "We couldn't process the AI's response. Please try again.";
                ViewBag.ErrorDetails = responseContent;
                return View("Error");
            }

            // Check if the workflow execution was successful
            if (workflowResponse.Status != "ok")
            {
                _logger.LogWarning("Workflow returned error status: {Message}", workflowResponse.Message);
                ViewBag.Error = "The AI encountered an error while processing your request.";
                ViewBag.ErrorDetails = workflowResponse.Message ?? "Unknown error";
                return View("Error");
            }

            // Pass data to the Result view
            ViewBag.OriginalPrompt = prompt;
            ViewBag.WorkflowId = workflowResponse.WorkflowId;
            ViewBag.Response = workflowResponse.Response;
            ViewBag.Model = workflowResponse.Metadata?.Model;
            ViewBag.Usage = workflowResponse.Metadata?.Usage;
            ViewBag.References = workflowResponse.References;
            ViewBag.RagEnabled = workflowResponse.Metadata?.RagEnabled ?? false;
            ViewData["AppName"] = _appSettings.Application?.Name ?? "AI Assistant";

            return View("Response");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request exception when calling n8n");
            ViewBag.Error = "We're having trouble connecting to our AI services. Please try again later.";
            ViewBag.ErrorDetails = $"Service unavailable at: {_configuration["N8N:BaseUrl"]}";
            return View("Error");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error when processing assistant request");
            ViewBag.Error = "An unexpected error occurred while processing your request.";
            ViewBag.ErrorDetails = ex.Message;
            return View("Error");
        }
    }
}

public class AppSettings
{
    public ApplicationSettings? Application { get; set; }
}

public class ApplicationSettings
{
    public string? Name { get; set; }
    public string? Version { get; set; }
    public string? Environment { get; set; }
}
