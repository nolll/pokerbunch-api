using System.Text.Json.Serialization;

namespace Api.Models.UserModels;

public class AddUserPostModel
{
    public string UserName { get; }
    public string DisplayName { get; }
    public string Email { get; }
    public string Password { get; }

    [JsonConstructor]
    public AddUserPostModel(string userName, string displayName, string email, string password)
    {
        UserName = userName;
        DisplayName = displayName;
        Email = email;
        Password = password;
    }
}