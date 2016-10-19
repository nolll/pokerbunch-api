using System.Web.Http;
using Api.Extensions;
using Api.Services;
using JetBrains.Annotations;

namespace Api.Controllers
{
    [EnsureHttps]
    [UsedImplicitly]
    public abstract class BaseApiController : ApiController
    {
        private readonly Bootstrapper _bootstrapper = new Bootstrapper(ApiSettings.ConnectionString);
        protected UseCaseContainer UseCase => _bootstrapper.UseCases;

        protected string CurrentUserName
        {
            get
            {
                if (User?.Identity == null)
                    return null;
                if (User.Identity.IsAuthenticated)
                    return User.Identity.Name;
                if (ApiSettings.AllowAuthOverride && Environment.IsNoAuthAdmin(Request.RequestUri.Host))
                    return ApiSettings.NoAuthAdminUserName;
                if (ApiSettings.AllowAuthOverride && Environment.IsNoAuthPlayer(Request.RequestUri.Host))
                    return ApiSettings.NoAuthPlayerUserName;
                return null;
            }
        }
    }

    public static class HttpVerb
    {
        public const string Get = "GET";
        public const string Post = "POST";
    }
}