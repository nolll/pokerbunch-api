using Api.Models.HomeModels;
using Api.Routes;
using Api.Settings;
using Api.Urls.ApiUrls;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : BaseController
{
    private readonly UrlProvider _urls;

    public HomeController(AppSettings appSettings, UrlProvider urls) : base(appSettings)
    {
        _urls = urls;
    }

    [Route(ApiRoutes.Root)]
    [HttpGet]
    [ProducesResponseType(typeof(HomeModel), 200)]
    public ObjectResult Home()
    {
        return Success(new HomeModel(_urls));
    }
}