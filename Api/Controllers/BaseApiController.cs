﻿using System.Web.Http;
using Api.Extensions;
using Api.Services;
using JetBrains.Annotations;
using Web.Common;
using Web.Common.Cache;

namespace Api.Controllers
{
    [EnsureHttps]
    [UsedImplicitly]
    public abstract class BaseApiController : ApiController
    {
        private const string DevAdminUserName = "henriks";
        private const string DevPlayerUserName = "kudden";

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
                if (Environment.IsNoAuthAdmin(Request.RequestUri.Host))
                    return DevAdminUserName;
                if (Environment.IsNoAuthPlayer(Request.RequestUri.Host))
                    return DevPlayerUserName;
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