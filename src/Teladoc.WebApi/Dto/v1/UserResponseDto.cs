using System.Text.Json.Serialization;

namespace Teladoc.WebApi.Dto.v1;

public class UserResponseDto
{
    [JsonPropertyName("FirstName")]
    public string FirstName { get; set; }
    
    [JsonPropertyName("LastName")]
    public string LastName { get; set; }
    
    [JsonPropertyName("Email")]
    public string Email { get; set; }
    
    [JsonPropertyName("DateOfBirth")]
    public DateTime DateOfBirth { get; set; }
    
    [JsonPropertyName("Nickname")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Nickname { get; set; }
    
    [JsonPropertyName("FriendCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? FriendCount { get; set; }
    
    [JsonPropertyName("Age")]
    public int Age { get; set; }
}