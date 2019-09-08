using System.Runtime.Serialization;
using Api.Urls.ApiUrls;

namespace Api.Models.HomeModels
{
    [DataContract(Namespace = "", Name = "user")]
    public class HomeModel
    {
        private readonly UrlProvider _urls;

        public HomeModel(UrlProvider urls)
        {
            _urls = urls;
        }

        [DataMember(Name = "userProfileUrl")]
        public string UserProfileUrl => _urls.Api.UserProfile.Absolute();
    }
}