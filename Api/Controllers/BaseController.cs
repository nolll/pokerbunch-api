using Api.Services;
using Api.Settings;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [UsedImplicitly]
    public abstract class BaseController : Controller
    {
        protected AppSettings AppSettings { get; }

        protected BaseController(AppSettings appSettings)
        {
            AppSettings = appSettings;
        }

        protected string CurrentUserName
        {
            get
            {
                if (User?.Identity == null)
                    return null;
                if (User.Identity.IsAuthenticated)
                    return User.Identity.Name;
                var env = new Environment(Request.Host.Host);
                if (AppSettings.Auth.Override.Enabled && env.IsDevModeAdmin)
                    return AppSettings.Auth.Override.AdminUserName;
                if (AppSettings.Auth.Override.Enabled && env.IsDevModePlayer)
                    return AppSettings.Auth.Override.PlayerUserName;
                return null;
            }
        }
    }
}