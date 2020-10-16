using JetBrains.Annotations;

namespace Api.Models.UserModels
{
    public class UpdateUserPostModel
    {
        public string DisplayName { get; [UsedImplicitly] set; }
        public string Email { get; [UsedImplicitly] set; }
        public string RealName { get; [UsedImplicitly] set; }
    }

    public class ChangePasswordPostModel
    {
        public string OldPassword { get; [UsedImplicitly] set; }
        public string Password { get; [UsedImplicitly] set; }
    }
}