using System.Text.Json.Serialization;

namespace N8nWorkflowIntegration.Models;

public class WorkflowResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
    
    [JsonPropertyName("workflowId")]
    public string? WorkflowId { get; set; }
    
    [JsonPropertyName("response")]
    public string? Response { get; set; }
    
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    
    [JsonPropertyName("metadata")]
    public ResponseMetadata? Metadata { get; set; }
    
    [JsonPropertyName("references")]
    public List<Reference>? References { get; set; }
}

public class ResponseMetadata
{
    [JsonPropertyName("model")]
    public string? Model { get; set; }
    
    [JsonPropertyName("usage")]
    public Usage? Usage { get; set; }
    
    [JsonPropertyName("timestamp")]
    public string? Timestamp { get; set; }
    
    [JsonPropertyName("ragEnabled")]
    public bool? RagEnabled { get; set; }
}

public class Usage
{
    [JsonPropertyName("prompt_tokens")]
    public int? PromptTokens { get; set; }
    
    [JsonPropertyName("completion_tokens")]
    public int? CompletionTokens { get; set; }
    
    [JsonPropertyName("total_tokens")]
    public int? TotalTokens { get; set; }
}

public class Reference
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    
    [JsonPropertyName("score")]
    public double? Score { get; set; }
    
    [JsonPropertyName("source")]
    public string? Source { get; set; }
    
    [JsonPropertyName("text")]
    public string? Text { get; set; }
}
