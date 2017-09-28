using System;
using System.Web.Http;
using Api.Auth;
using Api.Models.CashgameModels;
using Api.Models.CommonModels;
using Core.UseCases;
using PokerBunch.Common.Urls.ApiUrls;

namespace Api.Controllers
{
    public class CashgameController : BaseController
    {
        [Route(ApiCashgameUrl.Route)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameDetailsModel Get(int id)
        {
            var detailsRequest = new CashgameDetails.Request(CurrentUserName, id, DateTime.UtcNow);
            var detailsResult = UseCase.CashgameDetails.Execute(detailsRequest);
            return new CashgameDetailsModel(detailsResult);
        }

        [Route(ApiBunchCashgamesUrl.Route)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameListModel List(string bunchId)
        {
            var listResult = UseCase.CashgameList.Execute(new CashgameList.Request(CurrentUserName, bunchId, CashgameList.SortOrder.Date, null));
            return new CashgameListModel(listResult);
        }

        [Route(ApiBunchCashgamesUrl.RouteWithYear)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameListModel List(string bunchId, int year)
        {
            var listResult = UseCase.CashgameList.Execute(new CashgameList.Request(CurrentUserName, bunchId, CashgameList.SortOrder.Date, year));
            return new CashgameListModel(listResult);
        }

        [Route(ApiEventCashgamesUrl.Route)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameListModel EventCashgameList(int id)
        {
            var listResult = UseCase.EventCashgameList.Execute(new EventCashgameList.Request(CurrentUserName, id));
            return new CashgameListModel(listResult);
        }

        [Route(ApiPlayerCashgamesUrl.Route)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameListModel PlayerCashgameList(int id)
        {
            var listResult = UseCase.PlayerCashgameList.Execute(new PlayerCashgameList.Request(CurrentUserName, id));
            return new CashgameListModel(listResult);
        }

        [Route(ApiBunchCashgamesUrl.Route)]
        [HttpPost]
        [ApiAuthorize]
        public CashgameDetailsModel Add(string bunchId, [FromBody] AddCashgamePostModel post)
        {
            var addRequest = new AddCashgame.Request(CurrentUserName, bunchId, post.LocationId, post.EventId);
            var result = UseCase.AddCashgame.Execute(addRequest);
            var detailsRequest = new CashgameDetails.Request(CurrentUserName, result.CashgameId, DateTime.UtcNow);
            var detailsResult = UseCase.CashgameDetails.Execute(detailsRequest);
            return new CashgameDetailsModel(detailsResult);
        }

        [Route(ApiCashgameUrl.Route)]
        [HttpPut]
        [ApiAuthorize]
        public CashgameDetailsModel Update(int id, [FromBody] UpdateCashgamePostModel post)
        {
            var listRequest = new EditCashgame.Request(CurrentUserName, id, post.LocationId, post.EventId);
            UseCase.EditCashgame.Execute(listRequest);
            var detailsRequest = new CashgameDetails.Request(CurrentUserName, id, DateTime.UtcNow);
            var detailsResult = UseCase.CashgameDetails.Execute(detailsRequest);
            return new CashgameDetailsModel(detailsResult);
        }

        [Route(ApiCashgameUrl.Route)]
        [HttpDelete]
        [ApiAuthorize]
        public CashgameDeleteModel Delete(int id)
        {
            var deleteRequest = new DeleteCashgame.Request(CurrentUserName, id);
            UseCase.DeleteCashgame.Execute(deleteRequest);
            return new CashgameDeleteModel(id);
        }

        [Route(ApiBunchCashgamesCurrentUrl.Route)]
        [HttpGet]
        [ApiAuthorize]
        public CurrentCashgameListModel Current(string bunchId)
        {
            var currentGamesResult = UseCase.CurrentCashgames.Execute(new CurrentCashgames.Request(CurrentUserName, bunchId));
            return new CurrentCashgameListModel(currentGamesResult);
        }

        [Route(ApiCashgameBuyinUrl.Route)]
        [HttpPost]
        [ApiAuthorize]
        public OkModel Buyin(int id, [FromBody] CashgameBuyinPostModel post)
        {
            UseCase.Buyin.Execute(new Buyin.Request(CurrentUserName, id, post.PlayerId, post.Added, post.Stack, DateTime.UtcNow));
            return new OkModel();
        }

        [Route(ApiCashgameReportUrl.Route)]
        [HttpPost]
        [ApiAuthorize]
        public OkModel Report(int id, [FromBody] CashgameReportPostModel post)
        {
            UseCase.Report.Execute(new Report.Request(CurrentUserName, id, post.PlayerId, post.Stack, DateTime.UtcNow));
            return new OkModel();
        }

        [Route(ApiCashgameCashoutUrl.Route)]
        [HttpPost]
        [ApiAuthorize]
        public OkModel Cashout(int id, [FromBody] CashgameCashoutPostModel post)
        {
            UseCase.Cashout.Execute(new Cashout.Request(CurrentUserName, id, post.PlayerId, post.Stack, DateTime.UtcNow));
            return new OkModel();
        }

        [Route(ApiCashgameEndUrl.Route)]
        [HttpPost]
        [ApiAuthorize]
        public OkModel End(int id)
        {
            UseCase.EndCashgame.Execute(new EndCashgame.Request(CurrentUserName, id));
            return new OkModel();
        }

        [Route(ApiBunchCashgameYearsUrl.Route)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameYearListModel Years(string bunchId)
        {
            var listResult = UseCase.CashgameYearList.Execute(new CashgameYearList.Request(CurrentUserName, bunchId));
            return new CashgameYearListModel(listResult);
        }
    }
}