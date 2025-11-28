using Microsoft.AspNetCore.Mvc;
using N8nWorkflowIntegration.Models;
using System.Text;
using System.Text.Json;

namespace N8nWorkflowIntegration.Controllers;

public class WorkflowController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<WorkflowController> _logger;

    public WorkflowController(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<WorkflowController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// GET: Displays the Ask form
    /// </summary>
    [HttpGet]
    public IActionResult Ask()
    {
        return View();
    }

    /// <summary>
    /// POST: Sends the prompt to n8n workflow and displays the result
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ask(string prompt, string language = "en", bool useRag = false)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(prompt))
        {
            ViewBag.Error = "Please enter a prompt.";
            return View();
        }

        if (prompt.Length > 4000)
        {
            ViewBag.Error = "Prompt is too long. Maximum length is 4000 characters.";
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
                
                ViewBag.Error = $"Request failed with status {response.StatusCode}";
                ViewBag.ErrorDetails = responseContent;
                return View("Error");
            }

            // Deserialize the response
            var workflowResponse = JsonSerializer.Deserialize<WorkflowResponse>(responseContent);

            if (workflowResponse == null)
            {
                _logger.LogError("Failed to deserialize n8n response");
                ViewBag.Error = "Failed to parse the response from n8n.";
                ViewBag.ErrorDetails = responseContent;
                return View("Error");
            }

            // Check if the workflow execution was successful
            if (workflowResponse.Status != "ok")
            {
                _logger.LogWarning("Workflow returned error status: {Message}", workflowResponse.Message);
                ViewBag.Error = "Workflow execution failed.";
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

            return View("Result");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request exception when calling n8n");
            ViewBag.Error = "Failed to connect to n8n workflow.";
            ViewBag.ErrorDetails = $"Please ensure n8n is running and accessible at: {_configuration["N8N:BaseUrl"]}";
            return View("Error");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error when processing workflow request");
            ViewBag.Error = "An unexpected error occurred.";
            ViewBag.ErrorDetails = ex.Message;
            return View("Error");
        }
    }
}
