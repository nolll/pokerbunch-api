using System;
using Api.Auth;
using Api.Models.CashgameModels;
using Api.Models.CommonModels;
using Api.Routes;
using Core.Exceptions;
using Core.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ActionController : BaseController
    {
        public ActionController(Settings settings) : base(settings)
        {
        }

        [Route(ApiRoutes.Action.List)]
        [HttpPost]
        [ApiAuthorize]
        public OkModel Add(int cashgameId, [FromBody] AddCashgameActionPostModel post)
        {
            if(post.Type == ActionType.Buyin)
                return Buyin(cashgameId, post);
            if (post.Type == ActionType.Report)
                return Report(cashgameId, post);
            if(post.Type == ActionType.Cashout)
                return Cashout(cashgameId, post);

            throw new NotFoundException($"Action type not found. Valid types are [{ActionType.Buyin}], [{ActionType.Report}] and [{ActionType.Cashout}]");
        }

        private OkModel Buyin(int cashgameId, AddCashgameActionPostModel post)
        {
            UseCase.Buyin.Execute(new Buyin.Request(CurrentUserName, cashgameId, post.PlayerId, post.Added, post.Stack, DateTime.UtcNow));
            return new OkModel();
        }

        private OkModel Report(int cashgameId, AddCashgameActionPostModel post)
        {
            UseCase.Report.Execute(new Report.Request(CurrentUserName, cashgameId, post.PlayerId, post.Stack, DateTime.UtcNow));
            return new OkModel();
        }

        private OkModel Cashout(int cashgameId, AddCashgameActionPostModel post)
        {
            UseCase.Cashout.Execute(new Cashout.Request(CurrentUserName, cashgameId, post.PlayerId, post.Stack, DateTime.UtcNow));
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

        private static class ActionType
        {
            public const string Buyin = "buyin";
            public const string Report = "report";
            public const string Cashout = "cashout";
        }
    }
}