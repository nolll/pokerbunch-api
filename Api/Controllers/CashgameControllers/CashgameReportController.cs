using System;
using System.Web.Http;
using Api.Auth;
using Api.Routes;
using Core.Exceptions;
using Core.UseCases;
using JetBrains.Annotations;

namespace Api.Controllers.CashgameControllers
{
    public class CashgameReportController : BaseController
    {
        [Route(ApiRoutes.Report)]
        [HttpPost]
        [ApiAuthorize]
        public IHttpActionResult Report(string slug, [FromBody] CashgameReportPostModel post)
        {
            try
            {
                UseCase.Report.Execute(new Report.Request(CurrentUserName, slug, post.PlayerId, post.Stack, DateTime.UtcNow));
                return Ok();
            }
            catch (CashgameNotRunningException)
            {
                return InternalServerError();
            }
        }

        public class CashgameReportPostModel
        {
            public int PlayerId { get; [UsedImplicitly] set; }
            public int Stack { get; [UsedImplicitly] set; }
        }
    }
}