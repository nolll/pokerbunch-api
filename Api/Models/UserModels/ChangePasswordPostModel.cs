using JetBrains.Annotations;

namespace Api.Models.UserModels
{
    public class ChangePasswordPostModel
    {
        public string OldPassword { get; [UsedImplicitly] set; }
        public string Password { get; [UsedImplicitly] set; }
    }
}