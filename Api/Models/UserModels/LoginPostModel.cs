using JetBrains.Annotations;

namespace Api.Models.UserModels
{
    public class LoginPostModel
    {
        public string UserName { get; [UsedImplicitly] set; }
        public string Password { get; [UsedImplicitly] set; }
    }
}