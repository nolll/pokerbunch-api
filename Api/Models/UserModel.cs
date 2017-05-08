using System.Runtime.Serialization;
using Core.Entities;
using Core.UseCases;

namespace Api.Models
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

        public UserModel(UserDetails.Result r)
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

        [DataMember(Name = "fullName")]
        public string RealName { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        public FullUserModel(UserDetails.Result r)
            : base(r)
        {
            Role = r.Role.ToString();
            RealName = r.RealName;
            Email = r.Email;
        }
    }
}