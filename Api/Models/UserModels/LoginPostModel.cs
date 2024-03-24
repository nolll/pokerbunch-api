using System.Text.Json.Serialization;

namespace Api.Models.UserModels;

[method: JsonConstructor]
public class LoginPostModel(string userName, string password)
{
    public string UserName { get; } = userName;
    public string Password { get; } = password;
}