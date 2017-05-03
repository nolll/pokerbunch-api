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

        [DataMember(Name = "role")]
        public string Role { get; set; }

        public UserModel(UserDetails.Result r)
            : this(r.UserName, r.DisplayName, r.Role)
        {
        }

        private UserModel(string userName, string displayName, Role role)
        {
            UserName = userName;
            DisplayName = displayName;
            Role = role.ToString();
        }

        public UserModel()
        {
        }
    }
}