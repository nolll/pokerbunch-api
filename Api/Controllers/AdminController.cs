using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Models.AdminModels;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers
{
    public class AdminController : BaseController
    {
        [Route(ApiRoutes.Admin.ClearCache)]
        [HttpPost]
        [ApiAuthorize]
        public CacheClearedModel ClearCache()
        {
            var result = UseCase.ClearCache.Execute(new ClearCache.Request(CurrentUserName));
            return new CacheClearedModel(result.ClearCount);
        }

        [Route(ApiRoutes.Admin.SendEmail)]
        [HttpPost]
        [ApiAuthorize]
        public EmailSentModel SendEmail()
        {
            var result = UseCase.TestEmail.Execute(new TestEmail.Request(CurrentUserName));
            return new EmailSentModel(result);
        }
    }
}