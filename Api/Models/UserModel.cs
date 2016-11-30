using System.Runtime.Serialization;
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

        public UserModel(UserDetails.Result r)
            : this(r.UserName, r.DisplayName)
        {
        }

        private UserModel(string userName, string displayName)
        {
            UserName = userName;
            DisplayName = displayName;
        }

        public UserModel()
        {
        }
    }
}