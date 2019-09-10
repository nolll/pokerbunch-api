using Api.Auth;
using Api.Models.AdminModels;
using Api.Models.HomeModels;
using Api.Routes;
using Api.Settings;
using Core.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class AdminController : BaseController
    {
        private readonly AppSettings _appSettings;

        public AdminController(AppSettings appSettings) : base(appSettings)
        {
            _appSettings = appSettings;
        }

        /// <summary>
        /// Clears all cached data.
        /// </summary>
        [Route(ApiRoutes.Admin.ClearCache)]
        [HttpPost]
        [ApiAuthorize]
        public ClearCacheModel ClearCache()
        {
            var result = UseCase.ClearCache.Execute(new ClearCache.Request(CurrentUserName));
            return new ClearCacheModel(result.ClearCount);
        }

        /// <summary>
        /// Sends a test email.
        /// </summary>
        [Route(ApiRoutes.Admin.SendEmail)]
        [HttpPost]
        [ApiAuthorize]
        public SendEmailModel SendEmail()
        {
            var result = UseCase.TestEmail.Execute(new TestEmail.Request(CurrentUserName));
            return new SendEmailModel(result);
        }

        /// <summary>
        /// Gets the current build version of this api.
        /// </summary>
        [Route(ApiRoutes.Version)]
        [HttpGet]
        public VersionModel Version()
        {
            return new VersionModel(_appSettings.Version);
        }

        /// <summary>
        /// Gets the current application settings
        /// </summary>
        //[Route(ApiRoutes.Settings)]
        //[HttpGet]
        //public AppSettings Settings()
        //{
        //    return _appSettings;
        //}
    }
}