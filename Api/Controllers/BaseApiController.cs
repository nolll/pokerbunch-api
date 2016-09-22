using System.Web.Http;
using Api.Extensions;
using Web.Common;
using Web.Common.Cache;

namespace Api.Controllers
{
    [EnsureHttps]
    public abstract class BaseApiController : ApiController
    {
        private readonly Bootstrapper _bootstrapper = new Bootstrapper(ApiSettings.ConnectionString);
        protected UseCaseContainer UseCase => _bootstrapper.UseCases;

        protected string CurrentUserName
        {
            get
            {
                if (User == null || User.Identity == null)
                    return null;
                if (User.Identity.IsAuthenticated)
                    return User.Identity.Name;
                if (Environment.IsNoAuth(Request.RequestUri.Host))
                    return "henriks";
                if (Environment.IsDev(Request.RequestUri.Host))
                    return "henriks";
                return null;
            }
        }
    }
}