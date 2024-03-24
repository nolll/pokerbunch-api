using System.Text.Json.Serialization;

namespace Api.Models.UserModels;

[method: JsonConstructor]
public class AddUserPostModel(string userName, string displayName, string email, string password)
{
    public string UserName { get; } = userName;
    public string DisplayName { get; } = displayName;
    public string Email { get; } = email;
    public string Password { get; } = password;
}