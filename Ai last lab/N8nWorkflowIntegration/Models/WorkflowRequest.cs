using System.ComponentModel.DataAnnotations;

namespace N8nWorkflowIntegration.Models;

public class WorkflowRequest
{
    public string? UserId { get; set; }
    
    [Required(ErrorMessage = "Prompt is required")]
    [StringLength(4000, MinimumLength = 1, ErrorMessage = "Prompt must be between 1 and 4000 characters")]
    public string Prompt { get; set; } = string.Empty;
    
    public RequestMetadata? Metadata { get; set; }
}
