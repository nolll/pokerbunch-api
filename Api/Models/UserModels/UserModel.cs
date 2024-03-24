using System.Text.Json.Serialization;
using Core.UseCases;

namespace Api.Models.UserModels;

[method: JsonConstructor]
public class UserModel(string userName, string displayName, string avatar)
{
    [JsonPropertyName("userName")]
    public string UserName { get; } = userName;

    [JsonPropertyName("displayName")]
    public string DisplayName { get; } = displayName;

    [JsonPropertyName("avatar")]
    public string Avatar { get; } = avatar;

    public UserModel(UserDetails.Result r)
        : this(r.UserName, r.DisplayName, r.AvatarUrl)
    {
        UserName = r.UserName;
        DisplayName = r.DisplayName;
        Avatar = r.AvatarUrl;
    }
}