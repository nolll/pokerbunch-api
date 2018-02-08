using System;
using System.Web.Http;
using Api.Auth;
using Api.Models.CashgameModels;
using Api.Models.CommonModels;
using Core.UseCases;
using PokerBunch.Common.Routes;

namespace Api.Controllers
{
    public class ActionController : BaseController
    {
        [Route(ApiRoutes.Action.Buyin)]
        [HttpPost]
        [ApiAuthorize]
        public OkModel Buyin(int cashgameId, [FromBody] CashgameBuyinPostModel post)
        {
            UseCase.Buyin.Execute(new Buyin.Request(CurrentUserName, cashgameId, post.PlayerId, post.Added, post.Stack, DateTime.UtcNow));
            return new OkModel();
        }

        [Route(ApiRoutes.Action.Report)]
        [HttpPost]
        [ApiAuthorize]
        public OkModel Report(int cashgameId, [FromBody] CashgameReportPostModel post)
        {
            UseCase.Report.Execute(new Report.Request(CurrentUserName, cashgameId, post.PlayerId, post.Stack, DateTime.UtcNow));
            return new OkModel();
        }

        [Route(ApiRoutes.Action.Cashout)]
        [HttpPost]
        [ApiAuthorize]
        public OkModel Cashout(int cashgameId, [FromBody] CashgameCashoutPostModel post)
        {
            UseCase.Cashout.Execute(new Cashout.Request(CurrentUserName, cashgameId, post.PlayerId, post.Stack, DateTime.UtcNow));
            return new OkModel();
        }

        [Route(ApiRoutes.Action.End)]
        [HttpPost]
        [ApiAuthorize]
        public OkModel End(int cashgameId)
        {
            UseCase.EndCashgame.Execute(new EndCashgame.Request(CurrentUserName, cashgameId));
            return new OkModel();
        }

        [Route(ApiRoutes.Action.Get)]
        [HttpPut]
        [ApiAuthorize]
        public OkModel UpdateAction(int cashgameId, int actionId, [FromBody] UpdateActionPostModel post)
        {
            UseCase.EditCheckpoint.Execute(new EditCheckpoint.Request(CurrentUserName, actionId, post.Timestamp, post.Stack, post.Added));
            return new OkModel();
        }

        [Route(ApiRoutes.Action.Get)]
        [HttpDelete]
        [ApiAuthorize]
        public OkModel DeleteAction(int cashgameId, int actionId)
        {
            UseCase.DeleteCheckpoint.Execute(new DeleteCheckpoint.Request(CurrentUserName, actionId));
            return new OkModel();
        }
    }
}