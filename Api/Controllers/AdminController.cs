using Api.Auth;
using Api.Models.CommonModels;
using Api.Models.HomeModels;
using Api.Routes;
using Api.Settings;
using Core.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

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
    /// Clears all cached data.
    /// </summary>
    [Route(ApiRoutes.Admin.ClearCache)]
    [HttpPost]
    [ApiAuthorize]
    public async Task<ObjectResult> ClearCache()
    {
        var result = await _clearCache.Execute(new ClearCache.Request(CurrentUserName));
        return Model(result, () => new MessageModel(result.Data?.Message));
    }

    /// <summary>
    /// Sends a test email.
    /// </summary>
    [Route(ApiRoutes.Admin.SendEmail)]
    [HttpPost]
    [ApiAuthorize]
    public async Task<ObjectResult> SendEmail()
    {
        var result = await _testEmail.Execute(new TestEmail.Request(CurrentUserName));
        return Model(result, () => new MessageModel(result.Data?.Message));
    }

    /// <summary>
    /// Gets the current build version of this api.
    /// </summary>
    [Route(ApiRoutes.Version)]
    [HttpGet]
    public ObjectResult Version()
    {
        return Success(new VersionModel(_appSettings.Version));
    }

    /// <summary>
    /// Gets the current application settings
    /// </summary>
    [Route(ApiRoutes.Settings)]
    [HttpGet]
    [ApiAuthorize]
    public async Task<ObjectResult> Settings()
    {
        var result = await _requireAppsettingsAccess.Execute(new RequireAppsettingsAccess.Request(CurrentUserName));
        return Model(result, () => _appSettings);
    }
}