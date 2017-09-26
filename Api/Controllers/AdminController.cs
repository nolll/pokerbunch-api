using System.Web.Http;
using Api.Auth;
using Api.Models.AdminModels;
using Core.UseCases;
using PokerBunch.Common.Urls.ApiUrls;

namespace Api.Controllers
{
    public class AdminController : BaseController
    {
        [Route(ApiAdminClearCacheUrl.Route)]
        [HttpPost]
        [ApiAuthorize]
        public ClearCacheModel ClearCache()
        {
            var result = UseCase.ClearCache.Execute(new ClearCache.Request(CurrentUserName));
            return new ClearCacheModel(result.ClearCount);
        }

        [Route(ApiAdminSendEmailUrl.Route)]
        [HttpPost]
        [ApiAuthorize]
        public SendEmailModel SendEmail()
        {
            var result = UseCase.TestEmail.Execute(new TestEmail.Request(CurrentUserName));
            return new SendEmailModel(result);
        }
    }
}