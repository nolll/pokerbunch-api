using System.Runtime.Serialization;
using System.Web.Http;
using Api.Routes;
using Api.Urls.ApiUrls;

namespace Api.Controllers
{
    public class RootController : BaseController
    {
        [Route(ApiRoutes.Home)]
        [HttpGet]
        public HomeModel Home()
        {
            return new HomeModel();
        }
    }

    [DataContract(Namespace = "", Name = "user")]
    public class HomeModel
    {
        [DataMember(Name = "userProfileUrl")]
        public string UserProfileUrl => new ApiUserProfileUrl().Absolute;
    }
}