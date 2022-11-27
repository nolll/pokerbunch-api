using System.Text.Json.Serialization;

namespace Api.Models.UserModels;

public class ResetPasswordPostModel
{
    public string Email { get; }

    [JsonConstructor]
    public ResetPasswordPostModel(string email)
    {
        Email = email;
    }
}