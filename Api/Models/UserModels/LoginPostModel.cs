using System.Text.Json.Serialization;

namespace Api.Models.UserModels;

public class LoginPostModel
{
    public string UserName { get; }
    public string Password { get; }

    [JsonConstructor]
    public LoginPostModel(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }
}