using System.Web.Http;
using Api.Models.HomeModels;
using Api.Routes;

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
}