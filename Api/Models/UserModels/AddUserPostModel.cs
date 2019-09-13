using JetBrains.Annotations;

namespace Api.Models.UserModels
{
    public class AddUserPostModel
    {
        public string UserName { get; [UsedImplicitly] set; }
        public string DisplayName { get; [UsedImplicitly] set; }
        public string Email { get; [UsedImplicitly] set; }
        public string Password { get; [UsedImplicitly] set; }
    }
}