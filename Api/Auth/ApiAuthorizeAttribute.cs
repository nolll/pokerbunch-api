﻿using System.Web.Http;
using System.Web.Http.Controllers;
using Web.Common;

namespace Api.Auth
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (ApiSettings.RequireAuthorization)
                return true;
            if (Environment.IsNoAuth(actionContext.Request.RequestUri.Host))
                return true;
            return base.IsAuthorized(actionContext);
        }
    }
}