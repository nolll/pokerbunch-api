using System.Text.Json.Serialization;

namespace Api.Models.UserModels;

[method: JsonConstructor]
public class LoginModel(string accessToken, string refreshToken)
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; } = accessToken;
    
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; } = refreshToken;
}