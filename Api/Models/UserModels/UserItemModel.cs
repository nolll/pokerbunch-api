using System.Runtime.Serialization;
using Api.Urls.ApiUrls;
using Core.UseCases;

namespace Api.Models.UserModels
{
    [DataContract(Namespace = "", Name = "user")]
    public class UserItemModel
    {
        [DataMember(Name = "userName")]
        public string UserName { get; }

        [DataMember(Name = "displayName")]
        public string DisplayName { get; }

        [DataMember(Name = "url")]
        public string Url { get; }

        public UserItemModel(UserList.UserListItem r)
            : this(r.UserName, r.DisplayName)
        {
        }

        private UserItemModel(string userName, string displayName)
        {
            UserName = userName;
            DisplayName = displayName;
            Url = new ApiUserUrl(userName).Absolute;
        }
    }
}