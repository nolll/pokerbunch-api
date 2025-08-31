using Api.Models.CommonModels;
using Api.Models.HomeModels;
using Api.Routes;
using Api.Settings;
using Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class AdminController(
    AppSettings appSettings,
    ClearCache clearCache,
    TestEmail testEmail,
    RequireAppsettingsAccess requireAppsettingsAccess)
    : BaseController(appSettings)
{
    /// <summary>
    /// Clear cache
    /// </summary>
    [Route(ApiRoutes.Admin.ClearCache)]
    [HttpPost]
    [Authorize]
    public async Task<ObjectResult> ClearCache()
    {
        var result = await clearCache.Execute(new ClearCache.Request(Principal));
        return Model(result, () => new MessageModel(result.Data?.Message));
    }

    /// <summary>
    /// Send test email
    /// </summary>
    [Route(ApiRoutes.Admin.SendEmail)]
    [HttpPost]
    [Authorize]
    public async Task<ObjectResult> SendEmail()
    {
        var result = await testEmail.Execute(new TestEmail.Request(Principal));
        return Model(result, () => new MessageModel(result.Data?.Message));
    }

    [Route(ApiRoutes.Version)]
    [HttpGet]
    public ObjectResult Version() => Success(new VersionModel(AppSettings.Version));

    [Route(ApiRoutes.Settings)]
    [HttpGet]
    [Authorize]
    public async Task<ObjectResult> Settings()
    {
        var result = await requireAppsettingsAccess.Execute(new RequireAppsettingsAccess.Request(Principal));
        return Model(result, () => AppSettings);
    }
}