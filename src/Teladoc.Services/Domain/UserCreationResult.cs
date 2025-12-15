namespace Teladoc.Services.Domain;

public class UserCreationResult
{
    public bool Success { get; set; }
    public string[]? Errors { get; set; }
}