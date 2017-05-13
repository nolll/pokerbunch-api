using System;
using System.Web.Http;
using Api.Auth;
using Api.Routes;
using Core.Exceptions;
using Core.UseCases;
using JetBrains.Annotations;

namespace Api.Controllers.CashgameControllers
{
    public class CashgameCashoutController : BaseController
    {
        [Route(ApiRoutes.Cashout)]
        [HttpPost]
        [ApiAuthorize]
        public IHttpActionResult Cashout(string slug, [FromBody] CashgameCashoutPostModel postModel)
        {
            try
            {
                UseCase.Cashout.Execute(new Cashout.Request(CurrentUserName, slug, postModel.PlayerId, postModel.Stack, DateTime.UtcNow));
                return Ok();
            }
            catch (CashgameNotRunningException)
            {
                return InternalServerError();
            }
        }

        public class CashgameCashoutPostModel
        {
            public int PlayerId { get; [UsedImplicitly] set; }
            public int Stack { get; [UsedImplicitly] set; }
        }
    }
}