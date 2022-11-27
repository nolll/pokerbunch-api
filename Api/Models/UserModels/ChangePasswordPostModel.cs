using Newtonsoft.Json;

namespace Api.Models.UserModels;

public class ChangePasswordPostModel
{
    public string NewPassword { get; }
    public string OldPassword { get; }

    [JsonConstructor]
    public ChangePasswordPostModel(string newPassword, string oldPassword)
    {
        NewPassword = newPassword;
        OldPassword = oldPassword;
    }
}