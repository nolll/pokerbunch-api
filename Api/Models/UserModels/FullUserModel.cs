using System.Text.Json.Serialization;
using Core.UseCases;

namespace Api.Models.UserModels;

public class FullUserModel : UserModel
{
    [JsonPropertyName("role")]
    public string Role { get; }

    [JsonPropertyName("realName")]
    public string RealName { get; }

    [JsonPropertyName("email")]
    public string Email { get; }

    public FullUserModel(UserDetails.Result r)
        : base(r.UserName, r.DisplayName, r.AvatarUrl)
    {
        Role = r.Role.ToString().ToLower();
        RealName = r.RealName;
        Email = r.Email;
    }

    [JsonConstructor]
    public FullUserModel(string userName, string displayName, string avatar, string role, string realName, string email)
        : base(userName, displayName, avatar)
    {
        Role = role;
        RealName = realName;
        Email = email;
    }
}