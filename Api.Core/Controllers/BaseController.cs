using Api.Services;
using Api.Settings;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Plumbing;

namespace Api.Controllers
{
    [UsedImplicitly]
    public abstract class BaseController : Controller
    {
        private readonly Bootstrapper _bootstrapper;
        protected UseCaseContainer UseCase => _bootstrapper.UseCases;
        protected AppSettings AppSettings { get; }

        protected BaseController(AppSettings appSettings)
        {
            AppSettings = appSettings;
            var useSendGrid = AppSettings.Email.Provider == EmailProvider.SendGrid;
            _bootstrapper = new Bootstrapper(AppSettings.Sql.ConnectionString, AppSettings.Email.Smtp.Host, useSendGrid, AppSettings.Email.SendGrid.Key);
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