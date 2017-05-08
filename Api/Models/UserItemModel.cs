using System.Runtime.Serialization;
using Api.Extensions;
using Api.Urls.ApiUrls;
using Api.Urls.SiteUrls;
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

        [DataMember(Name = "url")]
        public string Url { get; set; }

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

        public UserItemModel()
        {
        }
    }
}