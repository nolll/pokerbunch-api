using System;
using System.Web.Http;
using Api.Auth;
using Api.Routes;
using Core.Exceptions;
using Core.UseCases;
using JetBrains.Annotations;

namespace Api.Controllers.CashgameControllers
{
    public class CashgameBuyinController : BaseController
    {
        [Route(ApiRoutes.Buyin)]
        [HttpPost]
        [ApiAuthorize]
        public IHttpActionResult Buyin(string slug, [FromBody] CashgameBuyinPostModel postModel)
        {
            try
            {
                UseCase.Buyin.Execute(new Buyin.Request(CurrentUserName, slug, postModel.PlayerId, postModel.Amount, postModel.Stack, DateTime.UtcNow));
                return Ok();
            }
            catch (CashgameNotRunningException)
            {
                return InternalServerError();
            }
        }

        public class CashgameBuyinPostModel
        {
            public int PlayerId { get; [UsedImplicitly] set; }
            public int Amount { get; [UsedImplicitly] set; }
            public int Stack { get; [UsedImplicitly] set; }
        }
    }
}