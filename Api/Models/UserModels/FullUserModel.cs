using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models.UserModels
{
    public class FullUserModel : UserModel
    {
        [DataMember(Name = "role")]
        public string Role { get; }

        [DataMember(Name = "realName")]
        public string RealName { get; }

        [DataMember(Name = "email")]
        public string Email { get; }

        public FullUserModel(UserDetails.Result r)
            : base(r.UserName, r.DisplayName, r.AvatarUrl)
        {
            Role = r.Role.ToString().ToLower();
            RealName = r.RealName;
            Email = r.Email;
        }
    }
}