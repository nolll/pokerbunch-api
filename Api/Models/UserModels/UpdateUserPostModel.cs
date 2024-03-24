using System.Text.Json.Serialization;

namespace Api.Models.UserModels;

[method: JsonConstructor]
public class UpdateUserPostModel(string displayName, string email, string? realName)
{
    public string DisplayName { get; } = displayName;
    public string Email { get; } = email;
    public string? RealName { get; } = realName;
}