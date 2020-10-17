using JetBrains.Annotations;

namespace Api.Models.UserModels
{
    public class ChangePasswordPostModel
    {
        public string Password { get; [UsedImplicitly] set; }
        public string OldPassword { get; [UsedImplicitly] set; }
    }
}