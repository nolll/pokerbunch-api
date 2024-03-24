using System.Text.Json.Serialization;
using Core.UseCases;

namespace Api.Models.UserModels;

[method: JsonConstructor]
public class FullUserModel(
    string userName,
    string displayName,
    string avatar,
    string role,
    string realName,
    string email)
    : UserModel(userName, displayName, avatar)
{
    [JsonPropertyName("role")]
    public string Role { get; } = role;

    [JsonPropertyName("realName")]
    public string RealName { get; } = realName;

    [JsonPropertyName("email")]
    public string Email { get; } = email;

    public FullUserModel(UserDetails.Result r)
        : this(r.UserName, r.DisplayName, r.AvatarUrl, r.Role.ToString().ToLower(), r.RealName, r.Email)
    {
    }
}