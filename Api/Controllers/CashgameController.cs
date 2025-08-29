using System.Collections.Generic;
using System.Linq;
using Api.Models.CashgameModels;
using Api.Routes;
using Api.Settings;
using Api.Urls.ApiUrls;
using Core.UseCases;
using Microsoft.AspNetCore.Authorization;
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
    /// <summary>
    /// Get a cashgame
    /// </summary>
    [Route(ApiRoutes.Cashgame.Get)]
    [HttpGet]
    [Authorize]
    public async Task<ObjectResult> Get(string cashgameId)
    {
        var request = new CashgameDetails.Request(AccessControl, cashgameId, DateTime.UtcNow);
        var result = await cashgameDetails.Execute(request);
        return Model(result, CreateModel);
        CashgameDetailsModel? CreateModel() => result.Data is not null ? new CashgameDetailsModel(result.Data) : null;
    }

    /// <summary>
    /// List cashgames
    /// </summary>
    [Route(ApiRoutes.Cashgame.ListByBunch)]
    [HttpGet]
    [Authorize]
    public async Task<ObjectResult> List(string bunchId)
    {
        var result = await cashgameList.Execute(new CashgameList.Request(CurrentUserName, bunchId, null));
        return Model(result, CreateModel);
        IEnumerable<CashgameListItemModel>? CreateModel() => result.Data?.Items.Select(o => new CashgameListItemModel(o));
    }

    /// <summary>
    /// List cashgames by year
    /// </summary>
    [Route(ApiRoutes.Cashgame.ListByBunchAndYear)]
    [HttpGet]
    [Authorize]
    public async Task<ObjectResult> List(string bunchId, int year)
    {
        var result = await cashgameList.Execute(new CashgameList.Request(CurrentUserName, bunchId, year));
        return Model(result, CreateModel);
        IEnumerable<CashgameListItemModel>? CreateModel() => result.Data?.Items.Select(o => new CashgameListItemModel(o));
    }

    /// <summary>
    /// List cashgames by event
    /// </summary>
    [Route(ApiRoutes.Cashgame.ListByEvent)]
    [HttpGet]
    [Authorize]
    public async Task<ObjectResult> EventCashgameList(string eventId)
    {
        var result = await eventCashgameList.Execute(new EventCashgameList.Request(CurrentUserName, eventId));
        return Model(result, () => result.Data?.Items.Select(o => new CashgameListItemModel(o)));
    }

    /// <summary>
    /// List cashgames by player
    /// </summary>
    [Route(ApiRoutes.Cashgame.ListByPlayer)]
    [HttpGet]
    [Authorize]
    public async Task<ObjectResult> PlayerCashgameList(string playerId)
    {
        var result = await playerCashgameList.Execute(new PlayerCashgameList.Request(CurrentUserName, playerId));
        return Model(result, () => result.Data?.Items.Select(o => new CashgameListItemModel(o)));
    }

    /// <summary>
    /// Add a cashgame
    /// </summary>
    [Route(ApiRoutes.Cashgame.Add)]
    [HttpPost]
    [Authorize]
    public async Task<ObjectResult> Add(string bunchId, [FromBody] AddCashgamePostModel post)
    {
        var addRequest = new AddCashgame.Request(AccessControl, bunchId, post.LocationId);
        var addResult = await addCashgame.Execute(addRequest);
        if (!addResult.Success)
            return Error(addResult.Error);

        var detailsRequest = new CashgameDetails.Request(AccessControl, addResult.Data?.CashgameId ?? "", DateTime.UtcNow);
        var detailsResult = await cashgameDetails.Execute(detailsRequest);
        return Model(detailsResult, () => detailsResult.Data is not null ? new CashgameDetailsModel(detailsResult.Data!) : null);
    }

    /// <summary>
    /// Update a cashgame
    /// </summary>
    [Route(ApiRoutes.Cashgame.Update)]
    [HttpPut]
    [Authorize]
    public async Task<ObjectResult> Update(string cashgameId, [FromBody] UpdateCashgamePostModel post)
    {
        var updateRequest = new EditCashgame.Request(AccessControl, cashgameId, post.LocationId, post.EventId);
        var updateResult = await editCashgame.Execute(updateRequest);
        if(!updateResult.Success)
            return Error(updateResult.Error);

        var detailsRequest = new CashgameDetails.Request(AccessControl, cashgameId, DateTime.UtcNow);
        var detailsResult = await cashgameDetails.Execute(detailsRequest);
        return Model(detailsResult, () => detailsResult.Data is not null ? new CashgameDetailsModel(detailsResult.Data!) : null);
    }

    /// <summary>
    /// Delete a cashgame
    /// </summary>
    [Route(ApiRoutes.Cashgame.Delete)]
    [HttpDelete]
    [Authorize]
    public async Task<ObjectResult> Delete(string cashgameId)
    {
        var request = new DeleteCashgame.Request(AccessControl, cashgameId);
        var result = await deleteCashgame.Execute(request);
        return Model(result, () => new CashgameDeletedModel(cashgameId));
    }

    /// <summary>
    /// List running cashgames
    /// </summary>
    [Route(ApiRoutes.Cashgame.ListCurrentByBunch)]
    [HttpGet]
    [Authorize]
    public async Task<ObjectResult> Current(string bunchId)
    {
        var result = await currentCashgames.Execute(new CurrentCashgames.Request(CurrentUserName, bunchId));
        return Model(result, () => result.Data?.Games.Select(o => new ApiCurrentGame(o, urls)));
    }
}