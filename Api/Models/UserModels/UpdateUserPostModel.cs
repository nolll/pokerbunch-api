using System.Text.Json.Serialization;

namespace Api.Models.UserModels;

public class UpdateUserPostModel
{
    public string DisplayName { get; }
    public string Email { get; }
    public string? RealName { get; }

    [JsonConstructor]
    public UpdateUserPostModel(string displayName, string email, string? realName)
    {
        DisplayName = displayName;
        Email = email;
        RealName = realName;
    }
}