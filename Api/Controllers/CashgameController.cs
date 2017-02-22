using System;
using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.Exceptions;
using Core.UseCases;
using JetBrains.Annotations;

namespace Api.Controllers
{
    public class CashgameController : BaseApiController
    {
        [Route("cashgame/toplist/{slug}/{year?}")]
        [HttpGet]
        [ApiAuthorize]
        public ApiCashgameTopList TopListAction(string slug, int? year = null)
        {
            var topListResult = UseCase.TopList.Execute(new TopList.Request(CurrentUserName, slug, year));
            return new ApiCashgameTopList(topListResult);
        }

        [Route(ApiRoutes.CurrentGames)]
        [HttpGet]
        [ApiAuthorize]
        public CurrentCashgameListModel Current(string slug)
        {
            var currentGamesResult = UseCase.CurrentCashgames.Execute(new CurrentCashgames.Request(CurrentUserName, slug));
            return new CurrentCashgameListModel(currentGamesResult);
        }

        [Route(ApiRoutes.CashgameList)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameListModel List(string slug, int? year = null)
        {
            var listResult = UseCase.CashgameList.Execute(new CashgameList.Request(CurrentUserName, slug, CashgameList.SortOrder.Date, year));
            return new CashgameListModel(listResult);
        }

        [Route(ApiRoutes.CashgameGet)]
        [HttpGet]
        [ApiAuthorize]
        public CashgameDetailsModel Get(int id)
        {
            var detailsRequest = new CashgameDetails.Request(CurrentUserName, id, DateTime.UtcNow);
            var detailsResult = UseCase.CashgameDetails.Execute(detailsRequest);
            return new CashgameDetailsModel(detailsResult);
        }

        [Route(ApiRoutes.CashgameGet)]
        [HttpPut]
        [ApiAuthorize]
        public CashgameDetailsModel Update(int id, [FromBody] UpdateCashgameObject c)
        {
            var listRequest = new EditCashgame.Request(CurrentUserName, id, c.locationid, c.eventid);
            UseCase.EditCashgame.Execute(listRequest);
            var detailsRequest = new CashgameDetails.Request(CurrentUserName, id, DateTime.UtcNow);
            var detailsResult = UseCase.CashgameDetails.Execute(detailsRequest);
            return new CashgameDetailsModel(detailsResult);
        }

        [Route(ApiRoutes.CashgameGet)]
        [HttpDelete]
        [ApiAuthorize]
        public CashgameDeletedModel Delete(int id)
        {
            var deleteRequest = new DeleteCashgame.Request(CurrentUserName, id);
            UseCase.DeleteCashgame.Execute(deleteRequest);
            return new CashgameDeletedModel(id);
        }

        public class UpdateCashgameObject
        {
            // ReSharper disable once InconsistentNaming
            public int locationid { get; [UsedImplicitly] set; }
            // ReSharper disable once InconsistentNaming
            public int eventid { get; [UsedImplicitly] set; }
        }

        [Route(ApiRoutes.Buyin)]
        [HttpPost]
        [ApiAuthorize]
        public IHttpActionResult Buyin(string slug, [FromBody] BuyinObject buyin)
        {
            try
            {
                UseCase.Buyin.Execute(new Buyin.Request(CurrentUserName, slug, buyin.playerid, buyin.amount, buyin.stack, DateTime.UtcNow));
                return Ok();
            }
            catch (CashgameNotRunningException)
            {
                return InternalServerError();
            }
        }

        public class BuyinObject
        {
            // ReSharper disable once InconsistentNaming
            public int playerid { get; [UsedImplicitly] set; }
            // ReSharper disable once InconsistentNaming
            public int amount { get; [UsedImplicitly] set; }
            // ReSharper disable once InconsistentNaming
            public int stack { get; [UsedImplicitly] set; }
        }

        [Route(ApiRoutes.Report)]
        [HttpPost]
        [ApiAuthorize]
        public IHttpActionResult Report(string slug, [FromBody] ReportObject report)
        {
            try
            {
                UseCase.Report.Execute(new Report.Request(CurrentUserName, slug, report.playerid, report.stack, DateTime.UtcNow));
                return Ok();
            }
            catch (CashgameNotRunningException)
            {
                return InternalServerError();
            }
        }

        public class ReportObject
        {
            // ReSharper disable once InconsistentNaming
            public int playerid { get; [UsedImplicitly] set; }
            // ReSharper disable once InconsistentNaming
            public int stack { get; [UsedImplicitly] set; }
        }

        [Route(ApiRoutes.Cashout)]
        [HttpPost]
        [ApiAuthorize]
        public IHttpActionResult Cashout(string slug, [FromBody] CashoutObject cashout)
        {
            try
            {
                UseCase.Cashout.Execute(new Cashout.Request(CurrentUserName, slug, cashout.playerid, cashout.stack, DateTime.UtcNow));
                return Ok();
            }
            catch (CashgameNotRunningException)
            {
                return InternalServerError();
            }
        }

        public class CashoutObject
        {
            // ReSharper disable once InconsistentNaming
            public int playerid { get; [UsedImplicitly] set; }
            // ReSharper disable once InconsistentNaming
            public int stack { get; [UsedImplicitly] set; }
        }
    }
}