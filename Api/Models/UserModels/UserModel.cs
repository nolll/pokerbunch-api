using System.Text.Json.Serialization;
using Core.UseCases;

namespace Api.Models.UserModels;

public class UserModel
{
    [JsonPropertyName("userName")]
    public string UserName { get; }

    [JsonPropertyName("displayName")]
    public string DisplayName { get; }

    [JsonPropertyName("avatar")]
    public string Avatar { get; }

    public UserModel(UserDetails.Result r)
        : this(r.UserName, r.DisplayName, r.AvatarUrl)
    {
        UserName = r.UserName;
        DisplayName = r.DisplayName;
        Avatar = r.AvatarUrl;
    }

    [JsonConstructor]
    public UserModel(string userName, string displayName, string avatar)
    {
        UserName = userName;
        DisplayName = displayName;
        Avatar = avatar;
    }
}