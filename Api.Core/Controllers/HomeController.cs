using Api.Models.HomeModels;
using Api.Routes;
using Api.Urls.ApiUrls;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class HomeController : BaseController
    {
        private readonly Settings _settings;
        private readonly UrlProvider _urls;

        public HomeController(Settings settings, UrlProvider urls) : base(settings)
        {
            _settings = settings;
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
            return new VersionModel(_settings.Version);
        }
    }
}