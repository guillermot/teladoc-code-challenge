using System.Text.Json.Serialization;

namespace Teladoc.WebApi.Dto.v1;

public class CreateUserDto
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
    public string? Nickname { get; set; }
    
    [JsonPropertyName("FriendCount")]
    public int? FriendCount { get; set; }
}