using Newtonsoft.Json;

namespace Api.Models.UserModels;

[method: JsonConstructor]
public class ChangePasswordPostModel(string newPassword, string oldPassword)
{
    public string NewPassword { get; } = newPassword;
    public string OldPassword { get; } = oldPassword;
}