using Api.Routes;
using Api.Settings;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : BaseController
{
    public HomeController(AppSettings appSettings) : base(appSettings)
    {
    }

    [Route(ApiRoutes.Root)]
    [HttpGet]
    public ActionResult Home()
    {
        return Redirect("/swagger/index.html");
    }
}