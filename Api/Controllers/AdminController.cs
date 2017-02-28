using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers
{
    public class AdminController : BaseApiController
    {
        [Route(ApiRoutes.Admin.SendEmail)]
        [HttpPost]
        [ApiAuthorize]
        public EmailSentModel SendEmail()
        {
            var result = UseCase.TestEmail.Execute(new TestEmail.Request(CurrentUserName));
            return new EmailSentModel(result);
        }

        [Route(ApiRoutes.Admin.ClearCache)]
        [HttpPost]
        [ApiAuthorize]
        public CacheClearedModel ClearCache()
        {
            var result = UseCase.ClearCache.Execute(new ClearCache.Request(CurrentUserName));
            return new CacheClearedModel(result.ClearCount);
        }
    }
}