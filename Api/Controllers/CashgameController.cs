using System.Linq;
using Api.Auth;
using Api.Models.CashgameModels;
using Api.Routes;
using Api.Settings;
using Api.Urls.ApiUrls;
using Core.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class CashgameController : BaseController
{
    private readonly UrlProvider _urls;
    private readonly CashgameDetails _cashgameDetails;
    private readonly CashgameList _cashgameList;
    private readonly EventCashgameList _eventCashgameList;
    private readonly PlayerCashgameList _playerCashgameList;
    private readonly AddCashgame _addCashgame;
    private readonly EditCashgame _editCashgame;
    private readonly DeleteCashgame _deleteCashgame;
    private readonly CurrentCashgames _currentCashgames;

    public CashgameController(
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
        : base(appSettings)
    {
        _urls = urls;
        _cashgameDetails = cashgameDetails;
        _cashgameList = cashgameList;
        _eventCashgameList = eventCashgameList;
        _playerCashgameList = playerCashgameList;
        _addCashgame = addCashgame;
        _editCashgame = editCashgame;
        _deleteCashgame = deleteCashgame;
        _currentCashgames = currentCashgames;
    }

    [Route(ApiRoutes.Cashgame.Get)]
    [HttpGet]
    [ApiAuthorize]
    public async Task<ObjectResult> Get(string cashgameId)
    {
        var request = new CashgameDetails.Request(CurrentUserName, cashgameId, DateTime.UtcNow);
        var result = await _cashgameDetails.Execute(request);
        return Model(result, () => new CashgameDetailsModel(result.Data));
    }

    [Route(ApiRoutes.Cashgame.ListByBunch)]
    [HttpGet]
    [ApiAuthorize]
    public async Task<ObjectResult> List(string bunchId)
    {
        var result = await _cashgameList.Execute(new CashgameList.Request(CurrentUserName, bunchId, CashgameList.SortOrder.Date, null));
        return Model(result, () => result.Data.Items.Select(o => new CashgameListItemModel(o)));
    }

    [Route(ApiRoutes.Cashgame.ListByBunchAndYear)]
    [HttpGet]
    [ApiAuthorize]
    public async Task<ObjectResult> List(string bunchId, int year)
    {
        var result = await _cashgameList.Execute(new CashgameList.Request(CurrentUserName, bunchId, CashgameList.SortOrder.Date, year));
        return Model(result, () => result.Data.Items.Select(o => new CashgameListItemModel(o)));
    }

    [Route(ApiRoutes.Cashgame.ListByEvent)]
    [HttpGet]
    [ApiAuthorize]
    public async Task<ObjectResult> EventCashgameList(string eventId)
    {
        var result = await _eventCashgameList.Execute(new EventCashgameList.Request(CurrentUserName, eventId));
        return Model(result, () => result.Data.Items.Select(o => new CashgameListItemModel(o)));
    }

    [Route(ApiRoutes.Cashgame.ListByPlayer)]
    [HttpGet]
    [ApiAuthorize]
    public async Task<ObjectResult> PlayerCashgameList(string playerId)
    {
        var result = await _playerCashgameList.Execute(new PlayerCashgameList.Request(CurrentUserName, playerId));
        return Model(result, () => result.Data.Items.Select(o => new CashgameListItemModel(o)));
    }

    [Route(ApiRoutes.Cashgame.Add)]
    [HttpPost]
    [ApiAuthorize]
    public async Task<ObjectResult> Add(string bunchId, [FromBody] AddCashgamePostModel post)
    {
        var addRequest = new AddCashgame.Request(CurrentUserName, bunchId, post.LocationId);
        var addResult = await _addCashgame.Execute(addRequest);
        if (!addResult.Success)
            return Error(addResult.Error);

        var detailsRequest = new CashgameDetails.Request(CurrentUserName, addResult.Data.CashgameId, DateTime.UtcNow);
        var detailsResult = await _cashgameDetails.Execute(detailsRequest);
        return Model(detailsResult, () => new CashgameDetailsModel(detailsResult.Data));
    }

    [Route(ApiRoutes.Cashgame.Update)]
    [HttpPut]
    [ApiAuthorize]
    public async Task<ObjectResult> Update(string cashgameId, [FromBody] UpdateCashgamePostModel post)
    {
        var updateRequest = new EditCashgame.Request(CurrentUserName, cashgameId, post.LocationId, post.EventId);
        var updateResult = await _editCashgame.Execute(updateRequest);
        if(!updateResult.Success)
            return Error(updateResult.Error);

        var detailsRequest = new CashgameDetails.Request(CurrentUserName, cashgameId, DateTime.UtcNow);
        var detailsResult = await _cashgameDetails.Execute(detailsRequest);
        return Model(detailsResult, () => new CashgameDetailsModel(detailsResult.Data));
    }

    [Route(ApiRoutes.Cashgame.Delete)]
    [HttpDelete]
    [ApiAuthorize]
    public async Task<ObjectResult> Delete(string cashgameId)
    {
        var request = new DeleteCashgame.Request(CurrentUserName, cashgameId);
        var result = await _deleteCashgame.Execute(request);
        return Model(result, () => new CashgameDeletedModel(cashgameId));
    }

    [Route(ApiRoutes.Cashgame.ListCurrentByBunch)]
    [HttpGet]
    [ApiAuthorize]
    public async Task<ObjectResult> Current(string bunchId)
    {
        var result = await _currentCashgames.Execute(new CurrentCashgames.Request(CurrentUserName, bunchId));
        return Model(result, () => result.Data.Games.Select(o => new ApiCurrentGame(o, _urls)));
    }
}