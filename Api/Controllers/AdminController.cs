using System.Web.Http;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers
{
    public class AdminController : BaseApiController
    {
        [Authorize]
        [Route(WebRoutes.Admin.SendEmail)]
        public EmailSentModel SendEmail()
        {
            var result = UseCase.TestEmail.Execute(new TestEmail.Request(CurrentUserName));
            return new EmailSentModel(result);
        }

        [Authorize]
        [Route(WebRoutes.Admin.ClearCache)]
        public CacheClearedModel ClearCache()
        {
            var result = UseCase.ClearCache.Execute(new ClearCache.Request(CurrentUserName));
            return new CacheClearedModel(result.ClearCount);
        }
    }
}