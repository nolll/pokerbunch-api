using System.Web.Http;
using Api.Models.HomeModels;
using PokerBunch.Common.Routes;

namespace Api.Controllers
{
    public class RootController : BaseController
    {
        [Route(ApiRoutes.Root)]
        [HttpGet]
        public HomeModel Home()
        {
            return new HomeModel();
        }
    }
}