using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models.UserModels
{
    [DataContract(Namespace = "", Name = "user")]
    public class UserModel
    {
        [DataMember(Name = "userName")]
        public string UserName { get; }

        [DataMember(Name = "displayName")]
        public string DisplayName { get; }

        [DataMember(Name = "avatar")]
        public string Avatar { get; }

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
    }
}