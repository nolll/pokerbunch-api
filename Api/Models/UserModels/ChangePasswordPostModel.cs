using JetBrains.Annotations;

namespace Api.Models.UserModels;

public class ChangePasswordPostModel
{
    public string NewPassword { get; [UsedImplicitly] set; }
    public string OldPassword { get; [UsedImplicitly] set; }
}