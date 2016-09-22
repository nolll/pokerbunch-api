using System.Web.Http;
using Api.Extensions;
using Web.Common.Routes;
using Web.Common.Urls.SiteUrls;

namespace Api.Controllers
{
    public class HomeController : BaseApiController
    {
        [Route(ApiRoutes.Home)]
        [AcceptVerbs("GET")]
        public IHttpActionResult Home()
        {
            return Redirect(new ApiDocsUrl().GetAbsolute());
        }
    }
}