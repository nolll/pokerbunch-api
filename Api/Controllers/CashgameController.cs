using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.Exceptions;
using Core.UseCases;
using JetBrains.Annotations;

namespace Api.Controllers
{
    public class NoContentResult<T> : NegotiatedContentResult<T>
    {
        public NoContentResult(T content, ApiController controller) : base(HttpStatusCode.NoContent, content, controller)
        {
        }
    }

    public class CashgameController : BaseApiController
    {
        protected NoContentResult<T> NoContent<T>(T content)
        {
            return new NoContentResult<T>(content, this);
        }

        [Route("cashgame/toplist/{slug}/{year?}")]
        [AcceptVerbs("GET")]
        [ApiAuthorize]
        public ApiCashgameTopList TopListAction(string slug, int? year = null)
        {
            var topListResult = UseCase.TopList.Execute(new TopList.Request(CurrentUserName, slug, year));
            return new ApiCashgameTopList(topListResult);
        }

        [Route(ApiRoutes.RunningGame)]
        [AcceptVerbs("GET")]
        [ApiAuthorize]
        public IHttpActionResult Running(string slug)
        {
            try
            {
                var runningResult = UseCase.RunningCashgame.Execute(new RunningCashgame.Request(CurrentUserName, slug));
                return Ok(new ApiRunning(runningResult));
            }
            catch (CashgameNotRunningException)
            {
                return Ok(new object());
            }
        }

        [Route(ApiRoutes.CashgameList)]
        [AcceptVerbs("GET")]
        [ApiAuthorize]
        public CashgameListModel List(string slug, int? year = null)
        {
            var listResult = UseCase.CashgameList.Execute(new CashgameList.Request(CurrentUserName, slug, CashgameList.SortOrder.Date, year));
            return new CashgameListModel(listResult);
        }
        
        [Route("cashgame/buyin/{slug}")]
        [AcceptVerbs("POST")]
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

        [Route("cashgame/report/{slug}")]
        [AcceptVerbs("POST")]
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

        [Route("cashgame/cashout/{slug}")]
        [AcceptVerbs("POST")]
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