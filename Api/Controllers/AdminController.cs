using Api.Auth;
using Api.Models.AdminModels;
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
    private readonly EnsureAdmin _ensureAdmin;

    public AdminController(AppSettings appSettings, ClearCache clearCache, TestEmail testEmail, EnsureAdmin ensureAdmin) : base(appSettings)
    {
        _appSettings = appSettings;
        _clearCache = clearCache;
        _testEmail = testEmail;
        _ensureAdmin = ensureAdmin;
    }

    /// <summary>
    /// Clears all cached data.
    /// </summary>
    [Route(ApiRoutes.Admin.ClearCache)]
    [HttpPost]
    [ApiAuthorize]
    public MessageModel ClearCache()
    {
        _clearCache.Execute(new ClearCache.Request(CurrentUserName));
        return new CacheClearedModel();
    }

    /// <summary>
    /// Sends a test email.
    /// </summary>
    [Route(ApiRoutes.Admin.SendEmail)]
    [HttpPost]
    [ApiAuthorize]
    public MessageModel SendEmail()
    {
        var result = _testEmail.Execute(new TestEmail.Request(CurrentUserName));
        return new EmailSentModel(result);
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
        _ensureAdmin.Execute(new EnsureAdmin.Request(CurrentUserName));

        return _appSettings;
    }
}