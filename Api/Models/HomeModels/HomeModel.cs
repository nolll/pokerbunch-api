using System.Runtime.Serialization;
using Api.Urls.ApiUrls;

namespace Api.Models.HomeModels
{
    [DataContract(Namespace = "", Name = "user")]
    public class HomeModel
    {
        [DataMember(Name = "userProfileUrl")]
        public string UserProfileUrl => new ApiUserProfileUrl().Absolute;
    }
}