using Api.Services;
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
        protected Settings Settings { get; }

        protected BaseController(Settings settings)
        {
            Settings = settings;
            _bootstrapper = new Bootstrapper(Settings.ConnectionString, Settings.SmtpHost, Settings.UseSendGrid, Settings.SendGridApiKey);
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
                if (Settings.AllowAuthOverride && env.IsDevModeAdmin)
                    return Settings.NoAuthAdminUserName;
                if (Settings.AllowAuthOverride && env.IsDevModePlayer)
                    return Settings.NoAuthPlayerUserName;
                return null;
            }
        }
    }
}