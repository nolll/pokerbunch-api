using JetBrains.Annotations;

namespace Api.Models.UserModels
{
    public class ResetPasswordPostModel
    {
        public string Email { get; [UsedImplicitly] set; }
    }
}