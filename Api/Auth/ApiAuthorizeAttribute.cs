using System.Web.Http;
using System.Web.Http.Controllers;
using Api.Services;

namespace Api.Auth
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (new Environment(actionContext.Request.RequestUri.Host).IsDevMode)
                return true;
            return base.IsAuthorized(actionContext);
        }
    }
}