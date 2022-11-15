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
    public ObjectResult ClearCache()
    {
        var result = _clearCache.Execute(new ClearCache.Request(CurrentUserName));

        return result.Success 
            ? Success(new MessageModel(result.Data.Message)) 
            : Error(result.Error);
    }

    /// <summary>
    /// Sends a test email.
    /// </summary>
    [Route(ApiRoutes.Admin.SendEmail)]
    [HttpPost]
    [ApiAuthorize]
    public ObjectResult SendEmail()
    {
        var result = _testEmail.Execute(new TestEmail.Request(CurrentUserName));
        
        return result.Success
            ? Success(new MessageModel(result.Data.Message))
            : Error(result.Error);
    }

    /// <summary>
    /// Gets the current build version of this api.
    /// </summary>
    [Route(ApiRoutes.Version)]
    [HttpGet]
    public VersionModel Version()
    {
        return new VersionModel(_appSettings.Version);
    }

    /// <summary>
    /// Gets the current application settings
    /// </summary>
    [Route(ApiRoutes.Settings)]
    [HttpGet]
    [ApiAuthorize]
    public AppSettings Settings()
    {
        _requireAppsettingsAccess.Execute(new RequireAppsettingsAccess.Request(CurrentUserName));

        return _appSettings;
    }
}