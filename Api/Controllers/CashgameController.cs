using System;
using System.Web.Http;
using Api.Auth;
using Api.Models.CashgameModels;
using Api.Models.CommonModels;
using Core.UseCases;
using PokerBunch.Common.Routes;

namespace Api.Controllers
{
    public class CashgameController : BaseController
    {
        [Route(ApiRoutes.Cashgame)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameDetailsModel Get(int cashgameId)
        {
            var detailsRequest = new CashgameDetails.Request(CurrentUserName, cashgameId, DateTime.UtcNow);
            var detailsResult = UseCase.CashgameDetails.Execute(detailsRequest);
            return new CashgameDetailsModel(detailsResult);
        }

        [Route(ApiRoutes.CashgamesByBunch)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameListModel List(string bunchId)
        {
            var listResult = UseCase.CashgameList.Execute(new CashgameList.Request(CurrentUserName, bunchId, CashgameList.SortOrder.Date, null));
            return new CashgameListModel(listResult);
        }

        [Route(ApiRoutes.CashgamesByBunchAndYear)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameListModel List(string bunchId, int year)
        {
            var listResult = UseCase.CashgameList.Execute(new CashgameList.Request(CurrentUserName, bunchId, CashgameList.SortOrder.Date, year));
            return new CashgameListModel(listResult);
        }

        [Route(ApiRoutes.CashgamesByEvent)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameListModel EventCashgameList(int eventId)
        {
            var listResult = UseCase.EventCashgameList.Execute(new EventCashgameList.Request(CurrentUserName, eventId));
            return new CashgameListModel(listResult);
        }

        [Route(ApiRoutes.CashgamesByPlayer)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameListModel PlayerCashgameList(int playerId)
        {
            var listResult = UseCase.PlayerCashgameList.Execute(new PlayerCashgameList.Request(CurrentUserName, playerId));
            return new CashgameListModel(listResult);
        }

        [Route(ApiRoutes.CashgamesByBunch)]
        [HttpPost]
        [ApiAuthorize]
        public CashgameDetailsModel Add(string bunchId, [FromBody] AddCashgamePostModel post)
        {
            var addRequest = new AddCashgame.Request(CurrentUserName, bunchId, post.LocationId);
            var result = UseCase.AddCashgame.Execute(addRequest);
            var detailsRequest = new CashgameDetails.Request(CurrentUserName, result.CashgameId, DateTime.UtcNow);
            var detailsResult = UseCase.CashgameDetails.Execute(detailsRequest);
            return new CashgameDetailsModel(detailsResult);
        }

        [Route(ApiRoutes.Cashgame)]
        [HttpPut]
        [ApiAuthorize]
        public CashgameDetailsModel Update(int cashgameId, [FromBody] UpdateCashgamePostModel post)
        {
            var listRequest = new EditCashgame.Request(CurrentUserName, cashgameId, post.LocationId, post.EventId);
            UseCase.EditCashgame.Execute(listRequest);
            var detailsRequest = new CashgameDetails.Request(CurrentUserName, cashgameId, DateTime.UtcNow);
            var detailsResult = UseCase.CashgameDetails.Execute(detailsRequest);
            return new CashgameDetailsModel(detailsResult);
        }

        [Route(ApiRoutes.Cashgame)]
        [HttpDelete]
        [ApiAuthorize]
        public CashgameDeleteModel Delete(int cashgameId)
        {
            var deleteRequest = new DeleteCashgame.Request(CurrentUserName, cashgameId);
            UseCase.DeleteCashgame.Execute(deleteRequest);
            return new CashgameDeleteModel(cashgameId);
        }

        [Route(ApiRoutes.CashgamesCurrentByBunch)]
        [HttpGet]
        [ApiAuthorize]
        public CurrentCashgameListModel Current(string bunchId)
        {
            var currentGamesResult = UseCase.CurrentCashgames.Execute(new CurrentCashgames.Request(CurrentUserName, bunchId));
            return new CurrentCashgameListModel(currentGamesResult);
        }

        [Route(ApiRoutes.ActionBuyin)]
        [HttpPost]
        [ApiAuthorize]
        public OkModel Buyin(int cashgameId, [FromBody] CashgameBuyinPostModel post)
        {
            UseCase.Buyin.Execute(new Buyin.Request(CurrentUserName, cashgameId, post.PlayerId, post.Added, post.Stack, DateTime.UtcNow));
            return new OkModel();
        }

        [Route(ApiRoutes.ActionReport)]
        [HttpPost]
        [ApiAuthorize]
        public OkModel Report(int cashgameId, [FromBody] CashgameReportPostModel post)
        {
            UseCase.Report.Execute(new Report.Request(CurrentUserName, cashgameId, post.PlayerId, post.Stack, DateTime.UtcNow));
            return new OkModel();
        }

        [Route(ApiRoutes.ActionCashout)]
        [HttpPost]
        [ApiAuthorize]
        public OkModel Cashout(int cashgameId, [FromBody] CashgameCashoutPostModel post)
        {
            UseCase.Cashout.Execute(new Cashout.Request(CurrentUserName, cashgameId, post.PlayerId, post.Stack, DateTime.UtcNow));
            return new OkModel();
        }

        [Route(ApiRoutes.ActionEnd)]
        [HttpPost]
        [ApiAuthorize]
        public OkModel End(int cashgameId)
        {
            UseCase.EndCashgame.Execute(new EndCashgame.Request(CurrentUserName, cashgameId));
            return new OkModel();
        }

        [Route(ApiRoutes.CashgameYearsByBunch)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameYearListModel Years(string bunchId)
        {
            var listResult = UseCase.CashgameYearList.Execute(new CashgameYearList.Request(CurrentUserName, bunchId));
            return new CashgameYearListModel(listResult);
        }

        [Route(ApiRoutes.Action)]
        [HttpPut]
        [ApiAuthorize]
        public OkModel UpdateAction(int cashgameId, int actionId, [FromBody] UpdateActionPostModel post)
        {
            UseCase.EditCheckpoint.Execute(new EditCheckpoint.Request(CurrentUserName, actionId, post.Timestamp, post.Stack, post.Added));
            return new OkModel();
        }

        [Route(ApiRoutes.Action)]
        [HttpDelete]
        [ApiAuthorize]
        public OkModel DeleteAction(int cashgameId, int actionId)
        {
            UseCase.DeleteCheckpoint.Execute(new DeleteCheckpoint.Request(CurrentUserName, actionId));
            return new OkModel();
        }
    }
}