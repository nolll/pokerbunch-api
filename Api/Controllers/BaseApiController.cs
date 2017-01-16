using System.Web.Http;
using Api.Extensions;
using Api.Services;
using JetBrains.Annotations;
using Plumbing;

namespace Api.Controllers
{
    [EnsureHttps]
    [UsedImplicitly]
    public abstract class BaseApiController : ApiController
    {
        private readonly Bootstrapper _bootstrapper = new Bootstrapper(Settings.ConnectionString);
        protected UseCaseContainer UseCase => _bootstrapper.UseCases;

        protected string CurrentUserName
        {
            get
            {
                if (User?.Identity == null)
                    return null;
                if (User.Identity.IsAuthenticated)
                    return User.Identity.Name;
                if (Settings.AllowAuthOverride && Environment.IsNoAuthAdmin(Request.RequestUri.Host))
                    return Settings.NoAuthAdminUserName;
                if (Settings.AllowAuthOverride && Environment.IsNoAuthPlayer(Request.RequestUri.Host))
                    return Settings.NoAuthPlayerUserName;
                return null;
            }
        }

        protected NoContentResult<T> NoContent<T>(T content)
        {
            return new NoContentResult<T>(content, this);
        }
    }

    public static class HttpVerb
    {
        public const string Get = "GET";
        public const string Post = "POST";
    }
}