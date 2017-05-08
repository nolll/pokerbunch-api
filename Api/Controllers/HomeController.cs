using System.Web.Http;
using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Controllers
{
    public class HomeController : BaseApiController
    {
        [Route(ApiRoutes.Home)]
        [HttpGet]
        public IHttpActionResult Home()
        {
            return Redirect(new ApiDocsUrl().Absolute);
        }
    }
}