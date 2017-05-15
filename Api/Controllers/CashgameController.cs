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
        [Route(ApiRoutes.CashgameListWithYear)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameListModel List(string slug, int? year = null)
        {
            var listResult = UseCase.CashgameList.Execute(new CashgameList.Request(CurrentUserName, slug, CashgameList.SortOrder.Date, year));
            return new CashgameListModel(listResult);
        }

        [Route(ApiRoutes.CashgameAdd)]
        [HttpPost]
        [ApiAuthorize]
        public CashgameDetailsModel Add(int id, [FromBody] AddCashgamePostModel c)
        {
            var listRequest = new EditCashgame.Request(CurrentUserName, id, c.LocationId, c.EventId);
            UseCase.EditCashgame.Execute(listRequest);
            var detailsRequest = new CashgameDetails.Request(CurrentUserName, id, DateTime.UtcNow);
            var detailsResult = UseCase.CashgameDetails.Execute(detailsRequest);
            return new CashgameDetailsModel(detailsResult);
        }

        [Route(ApiRoutes.CashgameUpdate)]
        [HttpPut]
        [ApiAuthorize]
        public CashgameDetailsModel Update(int id, [FromBody] UpdateCashgamePostModel c)
        {
            var listRequest = new EditCashgame.Request(CurrentUserName, id, c.LocationId, c.EventId);
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

        [Route(ApiRoutes.CurrentGames)]
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
        public OkModel Buyin(string slug, [FromBody] CashgameBuyinPostModel postModel)
        {
            UseCase.Buyin.Execute(new Buyin.Request(CurrentUserName, slug, postModel.PlayerId, postModel.Amount, postModel.Stack, DateTime.UtcNow));
            return new OkModel();
        }

        [Route(ApiRoutes.Report)]
        [HttpPost]
        [ApiAuthorize]
        public OkModel Report(string slug, [FromBody] CashgameReportPostModel post)
        {
            UseCase.Report.Execute(new Report.Request(CurrentUserName, slug, post.PlayerId, post.Stack, DateTime.UtcNow));
            return new OkModel();
        }

        [Route(ApiRoutes.Cashout)]
        [HttpPost]
        [ApiAuthorize]
        public OkModel Cashout(string slug, [FromBody] CashgameCashoutPostModel postModel)
        {
            UseCase.Cashout.Execute(new Cashout.Request(CurrentUserName, slug, postModel.PlayerId, postModel.Stack, DateTime.UtcNow));
            return new OkModel();
        }

        [Route(ApiRoutes.CashgameYears)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameYearListModel Years(string id)
        {
            var listResult = UseCase.CashgameYearList.Execute(new CashgameYearList.Request(CurrentUserName, id));
            return new CashgameYearListModel(listResult);
        }
    }
}