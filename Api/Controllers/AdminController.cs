using System.Web.Http;
using Api.Auth;
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
        public ClearCacheModel ClearCache()
        {
            var result = UseCase.ClearCache.Execute(new ClearCache.Request(CurrentUserName));
            return new ClearCacheModel(result.ClearCount);
        }

        [Route(ApiRoutes.Admin.SendEmail)]
        [HttpPost]
        [ApiAuthorize]
        public SendEmailModel SendEmail()
        {
            var result = UseCase.TestEmail.Execute(new TestEmail.Request(CurrentUserName));
            return new SendEmailModel(result);
        }
    }
}