using System.Web.Http;
using System.Web.Http.Controllers;
using Api.Services;

namespace Api.Auth
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (Environment.IsDevMode(actionContext.Request.RequestUri.Host))
                return true;
            return base.IsAuthorized(actionContext);
        }
    }
}