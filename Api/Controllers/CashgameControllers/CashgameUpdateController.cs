using System;
using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;
using JetBrains.Annotations;

namespace Api.Controllers.CashgameControllers
{
    public class CashgameUpdateController : BaseController
    {
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

        public class UpdateCashgamePostModel
        {
            public int LocationId { get; [UsedImplicitly] set; }
            public int EventId { get; [UsedImplicitly] set; }
        }
    }
}