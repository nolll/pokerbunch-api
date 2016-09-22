using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Api.Auth;
using Api.Models;
using Core.Exceptions;
using Core.UseCases;
using JetBrains.Annotations;

namespace Api.Controllers
{
    public class CashgameController : BaseApiController
    {
        [Route("cashgame/toplist/{slug}/{year?}")]
        [AcceptVerbs("GET")]
        [ApiAuthorize]
        public ApiCashgameTopList TopListAction(string slug, int? year = null)
        {
            var topListResult = UseCase.TopList.Execute(new TopList.Request(CurrentUserName, slug, year));
            return new ApiCashgameTopList(topListResult);
        }

        [Route("cashgame/running/{slug}")]
        [AcceptVerbs("GET")]
        [ApiAuthorize]
        public IHttpActionResult Running(string slug)
        {
            try
            {
                var runningResult = UseCase.RunningCashgame.Execute(new RunningCashgame.Request(CurrentUserName, slug));
                return Ok(new ApiRunning(runningResult));
            }
            catch (CashgameNotRunningException e)
            {
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.NoContent));
            }
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
            catch (CashgameNotRunningException e)
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
            catch (CashgameNotRunningException e)
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
            catch (CashgameNotRunningException e)
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