using System.Collections.Generic;
using System.Linq;
using Api.Models.CashgameModels;
using Api.Routes;
using Api.Settings;
using Api.Urls.ApiUrls;
using Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class CashgameController(
    AppSettings appSettings,
    UrlProvider urls,
    CashgameDetails cashgameDetails,
    CashgameList cashgameList,
    EventCashgameList eventCashgameList,
    PlayerCashgameList playerCashgameList,
    AddCashgame addCashgame,
    EditCashgame editCashgame,
    DeleteCashgame deleteCashgame,
    CurrentCashgames currentCashgames)
    : BaseController(appSettings)
{
    [Route(ApiRoutes.Cashgame.Get)]
    [HttpGet]
    [Authorize]
    [EndpointSummary("Get cashgame")]
    public async Task<ObjectResult> Get(string cashgameId)
    {
        var request = new CashgameDetails.Request(Principal, cashgameId, DateTime.UtcNow);
        var result = await cashgameDetails.Execute(request);
        return Model(result, CreateModel);
        CashgameDetailsModel? CreateModel() => result.Data is not null ? new CashgameDetailsModel(result.Data) : null;
    }
    
    [Route(ApiRoutes.Cashgame.ListByBunch)]
    [HttpGet]
    [Authorize]
    [EndpointSummary("List cashgames")]
    public async Task<ObjectResult> List(string bunchId)
    {
        var result = await cashgameList.Execute(new CashgameList.Request(Principal, bunchId, null));
        return Model(result, CreateModel);
        IEnumerable<CashgameListItemModel>? CreateModel() => result.Data?.Items.Select(o => new CashgameListItemModel(o));
    }
    
    [Route(ApiRoutes.Cashgame.ListByBunchAndYear)]
    [HttpGet]
    [Authorize]
    [EndpointSummary("List cashgames by year")]
    public async Task<ObjectResult> List(string bunchId, int year)
    {
        var result = await cashgameList.Execute(new CashgameList.Request(Principal, bunchId, year));
        return Model(result, CreateModel);
        IEnumerable<CashgameListItemModel>? CreateModel() => result.Data?.Items.Select(o => new CashgameListItemModel(o));
    }
    
    [Route(ApiRoutes.Cashgame.ListByEvent)]
    [HttpGet]
    [Authorize]
    [EndpointSummary("List cashgames by event")]
    public async Task<ObjectResult> EventCashgameList(string eventId)
    {
        var result = await eventCashgameList.Execute(new EventCashgameList.Request(Principal, eventId));
        return Model(result, () => result.Data?.Items.Select(o => new CashgameListItemModel(o)));
    }
    
    [Route(ApiRoutes.Cashgame.ListByPlayer)]
    [HttpGet]
    [Authorize]
    [EndpointSummary("List cashgames by player")]
    public async Task<ObjectResult> PlayerCashgameList(string playerId)
    {
        var result = await playerCashgameList.Execute(new PlayerCashgameList.Request(Principal, playerId));
        return Model(result, () => result.Data?.Items.Select(o => new CashgameListItemModel(o)));
    }
    
    [Route(ApiRoutes.Cashgame.Add)]
    [HttpPost]
    [Authorize]
    [EndpointSummary("Add cashgame")]
    public async Task<ObjectResult> Add(string bunchId, [FromBody] AddCashgamePostModel post)
    {
        var addRequest = new AddCashgame.Request(Principal, bunchId, post.LocationId);
        var addResult = await addCashgame.Execute(addRequest);
        if (!addResult.Success)
            return Error(addResult.Error);

        var detailsRequest = new CashgameDetails.Request(Principal, addResult.Data?.CashgameId ?? "", DateTime.UtcNow);
        var detailsResult = await cashgameDetails.Execute(detailsRequest);
        return Model(detailsResult, () => detailsResult.Data is not null ? new CashgameDetailsModel(detailsResult.Data!) : null);
    }
    
    [Route(ApiRoutes.Cashgame.Update)]
    [HttpPut]
    [Authorize]
    [EndpointSummary("Update cashgame")]
    public async Task<ObjectResult> Update(string cashgameId, [FromBody] UpdateCashgamePostModel post)
    {
        var updateRequest = new EditCashgame.Request(Principal, cashgameId, post.LocationId, post.EventId);
        var updateResult = await editCashgame.Execute(updateRequest);
        if(!updateResult.Success)
            return Error(updateResult.Error);

        var detailsRequest = new CashgameDetails.Request(Principal, cashgameId, DateTime.UtcNow);
        var detailsResult = await cashgameDetails.Execute(detailsRequest);
        return Model(detailsResult, () => detailsResult.Data is not null ? new CashgameDetailsModel(detailsResult.Data!) : null);
    }
    
    [Route(ApiRoutes.Cashgame.Delete)]
    [HttpDelete]
    [Authorize]
    [EndpointSummary("Delete cashgame")]
    public async Task<ObjectResult> Delete(string cashgameId)
    {
        var request = new DeleteCashgame.Request(Principal, cashgameId);
        var result = await deleteCashgame.Execute(request);
        return Model(result, () => new CashgameDeletedModel(cashgameId));
    }
    
    [Route(ApiRoutes.Cashgame.ListCurrentByBunch)]
    [HttpGet]
    [Authorize]
    [EndpointSummary("List running cashgames")]
    public async Task<ObjectResult> Current(string bunchId)
    {
        var result = await currentCashgames.Execute(new CurrentCashgames.Request(Principal, bunchId));
        return Model(result, () => result.Data?.Games.Select(o => new ApiCurrentGame(o, urls)));
    }
}