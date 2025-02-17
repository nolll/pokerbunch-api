using Api.Routes;
using Api.Settings;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController(AppSettings appSettings) : BaseController(appSettings)
{
    [Route(ApiRoutes.Root)]
    [HttpGet]
    public ActionResult Home() => Redirect("/swagger/index.html");
}