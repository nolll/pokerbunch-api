using System.Web.Http;
using Api.Models.HomeModels;
using PokerBunch.Common.Urls.ApiUrls;

namespace Api.Controllers
{
    public class RootController : BaseController
    {
        [Route(ApiRootUrl.Route)]
        [HttpGet]
        public HomeModel Home()
        {
            return new HomeModel();
        }
    }
}