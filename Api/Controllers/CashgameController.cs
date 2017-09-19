using System;
using System.Web.Http;
using Api.Auth;
using Api.Models.CashgameModels;
using Api.Models.CommonModels;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers
{
    public class CashgameController : BaseController
    {
        [Route(ApiRoutes.CashgameGet)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameDetailsModel Get(int id)
        {
            var detailsRequest = new CashgameDetails.Request(CurrentUserName, id, DateTime.UtcNow);
            var detailsResult = UseCase.CashgameDetails.Execute(detailsRequest);
            return new CashgameDetailsModel(detailsResult);
        }

        [Route(ApiRoutes.CashgameList)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameListModel List(string slug)
        {
            var listResult = UseCase.CashgameList.Execute(new CashgameList.Request(CurrentUserName, slug, CashgameList.SortOrder.Date, null));
            return new CashgameListModel(listResult);
        }

        [Route(ApiRoutes.CashgameListWithYear)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameListModel List(string slug, int year)
        {
            var listResult = UseCase.CashgameList.Execute(new CashgameList.Request(CurrentUserName, slug, CashgameList.SortOrder.Date, year));
            return new CashgameListModel(listResult);
        }

        [Route(ApiRoutes.EventCashgameList)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameListModel EventCashgameList(int id)
        {
            var listResult = UseCase.EventCashgameList.Execute(new EventCashgameList.Request(CurrentUserName, id));
            return new CashgameListModel(listResult);
        }

        [Route(ApiRoutes.PlayerCashgameList)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameListModel PlayerCashgameList(int id)
        {
            var listResult = UseCase.PlayerCashgameList.Execute(new PlayerCashgameList.Request(CurrentUserName, id));
            return new CashgameListModel(listResult);
        }

        [Route(ApiRoutes.CashgameAdd)]
        [HttpPost]
        [ApiAuthorize]
        public CashgameDetailsModel Add(string slug, [FromBody] AddCashgamePostModel post)
        {
            var addRequest = new AddCashgame.Request(CurrentUserName, slug, post.LocationId, post.EventId);
            var result = UseCase.AddCashgame.Execute(addRequest);
            var detailsRequest = new CashgameDetails.Request(CurrentUserName, result.CashgameId, DateTime.UtcNow);
            var detailsResult = UseCase.CashgameDetails.Execute(detailsRequest);
            return new CashgameDetailsModel(detailsResult);
        }

        [Route(ApiRoutes.CashgameUpdate)]
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

        [Route(ApiRoutes.CashgameDelete)]
        [HttpDelete]
        [ApiAuthorize]
        public CashgameDeleteModel Delete(int id)
        {
            var deleteRequest = new DeleteCashgame.Request(CurrentUserName, id);
            UseCase.DeleteCashgame.Execute(deleteRequest);
            return new CashgameDeleteModel(id);
        }

        [Route(ApiRoutes.BunchCurrentGames)]
        [HttpGet]
        [ApiAuthorize]
        public CurrentCashgameListModel Current(string slug)
        {
            var currentGamesResult = UseCase.CurrentCashgames.Execute(new CurrentCashgames.Request(CurrentUserName, slug));
            return new CurrentCashgameListModel(currentGamesResult);
        }

        [Route(ApiRoutes.Buyin)]
        [HttpPost]
        [ApiAuthorize]
        public OkModel Buyin(int id, [FromBody] CashgameBuyinPostModel post)
        {
            UseCase.Buyin.Execute(new Buyin.Request(CurrentUserName, id, post.PlayerId, post.Added, post.Stack, DateTime.UtcNow));
            return new OkModel();
        }

        [Route(ApiRoutes.Report)]
        [HttpPost]
        [ApiAuthorize]
        public OkModel Report(int id, [FromBody] CashgameReportPostModel post)
        {
            UseCase.Report.Execute(new Report.Request(CurrentUserName, id, post.PlayerId, post.Stack, DateTime.UtcNow));
            return new OkModel();
        }

        [Route(ApiRoutes.Cashout)]
        [HttpPost]
        [ApiAuthorize]
        public OkModel Cashout(int id, [FromBody] CashgameCashoutPostModel post)
        {
            UseCase.Cashout.Execute(new Cashout.Request(CurrentUserName, id, post.PlayerId, post.Stack, DateTime.UtcNow));
            return new OkModel();
        }

        [Route(ApiRoutes.EndCashgame)]
        [HttpPost]
        [ApiAuthorize]
        public OkModel End(int id)
        {
            UseCase.EndCashgame.Execute(new EndCashgame.Request(CurrentUserName, id));
            return new OkModel();
        }

        [Route(ApiRoutes.CashgameYears)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameYearListModel Years(string slug)
        {
            var listResult = UseCase.CashgameYearList.Execute(new CashgameYearList.Request(CurrentUserName, slug));
            return new CashgameYearListModel(listResult);
        }
    }
}