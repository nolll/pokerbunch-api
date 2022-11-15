using Api.Models.HomeModels;
using Api.Routes;
using Api.Settings;
using Api.Urls.ApiUrls;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class HomeController : BaseController
{
    private readonly UrlProvider _urls;

    public HomeController(AppSettings appSettings, UrlProvider urls) : base(appSettings)
    {
        _urls = urls;
    }

    /// <summary>
    /// The root of this api.
    /// </summary>
    [Route(ApiRoutes.Root)]
    [HttpGet]
    public ObjectResult Home()
    {
        return Success(new HomeModel(_urls));
    }
}