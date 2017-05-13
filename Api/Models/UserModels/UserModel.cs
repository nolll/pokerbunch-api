using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models.UserModels
{
    [DataContract(Namespace = "", Name = "user")]
    public class UserModel
    {
        [DataMember(Name = "userName")]
        public string UserName { get; set; }

        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }

        [DataMember(Name = "avatar")]
        public string Avatar { get; set; }

        public UserModel(string userName, string displayName, string avatarUrl)
        {
            UserName = userName;
            DisplayName = displayName;
            Avatar = avatarUrl;
        }

        public UserModel(UserDetails.Result r)
            : this(r.UserName, r.DisplayName, r.AvatarUrl)
        {
            UserName = r.UserName;
            DisplayName = r.DisplayName;
            Avatar = r.AvatarUrl;
        }

        public UserModel()
        {
        }
    }

    public class FullUserModel : UserModel
    {
        [DataMember(Name = "role")]
        public string Role { get; set; }

        [DataMember(Name = "realName")]
        public string RealName { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        public FullUserModel(UserDetails.Result r)
            : base(r.UserName, r.DisplayName, r.AvatarUrl)
        {
            Role = r.Role.ToString();
            RealName = r.RealName;
            Email = r.Email;
        }
    }
}