using Api.Models.HomeModels;
using Api.Routes;
using Api.Urls.ApiUrls;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class RootController : BaseController
    {
        private readonly UrlProvider _urls;

        public RootController(Settings settings, UrlProvider urls) : base(settings)
        {
            _urls = urls;
        }

        [Route(ApiRoutes.Root)]
        [HttpGet]
        public HomeModel Home()
        {
            return new HomeModel(_urls);
        }

        [Route(ApiRoutes.Version)]
        [HttpGet]
        public VersionModel Version()
        {
            return new VersionModel();
        }
    }
}