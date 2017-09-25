using System.Runtime.Serialization;
using Api.Extensions;
using Core.UseCases;
using PokerBunch.Common.Urls.ApiUrls;

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
            Url = new ApiUserUrl(userName).Absolute();
        }
    }
}