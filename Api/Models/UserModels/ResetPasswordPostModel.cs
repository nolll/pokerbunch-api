using System.Text.Json.Serialization;

namespace Api.Models.UserModels;

[method: JsonConstructor]
public class ResetPasswordPostModel(string email)
{
    public string Email { get; } = email;
}