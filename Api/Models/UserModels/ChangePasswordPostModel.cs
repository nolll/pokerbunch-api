using System.Text.Json.Serialization;

namespace Api.Models.UserModels;

[method: JsonConstructor]
public class ChangePasswordPostModel(string newPassword, string oldPassword)
{
    public string NewPassword { get; } = newPassword;
    public string OldPassword { get; } = oldPassword;
}