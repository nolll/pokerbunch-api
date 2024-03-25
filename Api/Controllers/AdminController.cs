using Api.Models.CommonModels;
using Api.Models.HomeModels;
using Api.Routes;
using Api.Settings;
using Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class AdminController : BaseController
{
    private readonly AppSettings _appSettings;
    private readonly ClearCache _clearCache;
    private readonly TestEmail _testEmail;
    private readonly RequireAppsettingsAccess _requireAppsettingsAccess;

    public AdminController(AppSettings appSettings, ClearCache clearCache, TestEmail testEmail, RequireAppsettingsAccess requireAppsettingsAccess) : base(appSettings)
    {
        _appSettings = appSettings;
        _clearCache = clearCache;
        _testEmail = testEmail;
        _requireAppsettingsAccess = requireAppsettingsAccess;
    }

    /// <summary>
    /// Clear cache
    /// </summary>
    [Route(ApiRoutes.Admin.ClearCache)]
    [HttpPost]
    [Authorize]
    public async Task<ObjectResult> ClearCache()
    {
        var result = await _clearCache.Execute(new ClearCache.Request(CurrentUserName));
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
        var result = await _testEmail.Execute(new TestEmail.Request(CurrentUserName));
        return Model(result, () => new MessageModel(result.Data?.Message));
    }

    [Route(ApiRoutes.Version)]
    [HttpGet]
    public ObjectResult Version()
    {
        return Success(new VersionModel(_appSettings.Version));
    }

    [Route(ApiRoutes.Settings)]
    [HttpGet]
    [Authorize]
    public async Task<ObjectResult> Settings()
    {
        var result = await _requireAppsettingsAccess.Execute(new RequireAppsettingsAccess.Request(CurrentUserName));
        return Model(result, () => _appSettings);
    }
}