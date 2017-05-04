using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [DataContract(Namespace = "", Name = "user")]
    public class UserItemModel
    {
        [DataMember(Name = "userName")]
        public string UserName { get; set; }

        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }

        public UserItemModel(UserList.UserListItem r)
            : this(r.UserName, r.DisplayName)
        {
        }

        private UserItemModel(string userName, string displayName)
        {
            UserName = userName;
            DisplayName = displayName;
        }

        public UserItemModel()
        {
        }
    }
}