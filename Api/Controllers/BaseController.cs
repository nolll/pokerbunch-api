using System.Web.Http;
using Api.Extensions;
using Api.Services;
using JetBrains.Annotations;
using Plumbing;

namespace Api.Controllers
{
    [EnsureHttps]
    [UsedImplicitly]
    public abstract class BaseController : ApiController
    {
        private readonly Bootstrapper _bootstrapper = new Bootstrapper(Settings.ConnectionString, Settings.SmtpHost, Settings.SmtpUserName, Settings.SmtpPassword);
        protected UseCaseContainer UseCase => _bootstrapper.UseCases;

        protected string CurrentUserName
        {
            get
            {
                if (User?.Identity == null)
                    return null;
                if (User.Identity.IsAuthenticated)
                    return User.Identity.Name;
                if (Settings.AllowAuthOverride && Environment.IsDevModeAdmin(Request.RequestUri.Host))
                    return Settings.NoAuthAdminUserName;
                if (Settings.AllowAuthOverride && Environment.IsDevModePlayer(Request.RequestUri.Host))
                    return Settings.NoAuthPlayerUserName;
                return null;
            }
        }

        protected NoContentResult<T> NoContent<T>(T content)
        {
            return new NoContentResult<T>(content, this);
        }
    }
}