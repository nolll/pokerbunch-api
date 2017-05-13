using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers.AdminControllers
{
    public class SendEmailController : BaseController
    {
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