using System;
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
    private readonly CashgameYearList _cashgameYearList;

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
        CurrentCashgames currentCashgames,
        CashgameYearList cashgameYearList) 
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
        _cashgameYearList = cashgameYearList;
    }

    [Route(ApiRoutes.Cashgame.Get)]
    [HttpGet]
    [ApiAuthorize]
    public CashgameDetailsModel Get(int cashgameId)
    {
        var detailsRequest = new CashgameDetails.Request(CurrentUserName, cashgameId, DateTime.UtcNow);
        var detailsResult = _cashgameDetails.Execute(detailsRequest);
        return new CashgameDetailsModel(detailsResult);
    }

    [Route(ApiRoutes.Cashgame.ListByBunch)]
    [HttpGet]
    [ApiAuthorize]
    public CashgameListModel List(string bunchId)
    {
        var listResult = _cashgameList.Execute(new CashgameList.Request(CurrentUserName, bunchId, CashgameList.SortOrder.Date, null));
        return new CashgameListModel(listResult);
    }

    [Route(ApiRoutes.Cashgame.ListByBunchAndYear)]
    [HttpGet]
    [ApiAuthorize]
    public CashgameListModel List(string bunchId, int year)
    {
        var listResult = _cashgameList.Execute(new CashgameList.Request(CurrentUserName, bunchId, CashgameList.SortOrder.Date, year));
        return new CashgameListModel(listResult);
    }

    [Route(ApiRoutes.Cashgame.ListByEvent)]
    [HttpGet]
    [ApiAuthorize]
    public CashgameListModel EventCashgameList(int eventId)
    {
        var listResult = _eventCashgameList.Execute(new EventCashgameList.Request(CurrentUserName, eventId));
        return new CashgameListModel(listResult);
    }

    [Route(ApiRoutes.Cashgame.ListByPlayer)]
    [HttpGet]
    [ApiAuthorize]
    public CashgameListModel PlayerCashgameList(int playerId)
    {
        var listResult = _playerCashgameList.Execute(new PlayerCashgameList.Request(CurrentUserName, playerId));
        return new CashgameListModel(listResult);
    }

    [Route(ApiRoutes.Cashgame.ListByBunch)]
    [HttpPost]
    [ApiAuthorize]
    public CashgameDetailsModel Add(string bunchId, [FromBody] AddCashgamePostModel post)
    {
        var addRequest = new AddCashgame.Request(CurrentUserName, bunchId, post.LocationId);
        var result = _addCashgame.Execute(addRequest);
        var detailsRequest = new CashgameDetails.Request(CurrentUserName, result.CashgameId, DateTime.UtcNow);
        var detailsResult = _cashgameDetails.Execute(detailsRequest);
        return new CashgameDetailsModel(detailsResult);
    }

    [Route(ApiRoutes.Cashgame.Get)]
    [HttpPut]
    [ApiAuthorize]
    public CashgameDetailsModel Update(int cashgameId, [FromBody] UpdateCashgamePostModel post)
    {
        var listRequest = new EditCashgame.Request(CurrentUserName, cashgameId, post.LocationId, post.EventId);
        _editCashgame.Execute(listRequest);
        var detailsRequest = new CashgameDetails.Request(CurrentUserName, cashgameId, DateTime.UtcNow);
        var detailsResult = _cashgameDetails.Execute(detailsRequest);
        return new CashgameDetailsModel(detailsResult);
    }

    [Route(ApiRoutes.Cashgame.Get)]
    [HttpDelete]
    [ApiAuthorize]
    public CashgameDeleteModel Delete(int cashgameId)
    {
        var deleteRequest = new DeleteCashgame.Request(CurrentUserName, cashgameId);
        _deleteCashgame.Execute(deleteRequest);
        return new CashgameDeleteModel(cashgameId);
    }

    [Route(ApiRoutes.Cashgame.ListCurrentByBunch)]
    [HttpGet]
    [ApiAuthorize]
    public CurrentCashgameListModel Current(string bunchId)
    {
        var currentGamesResult = _currentCashgames.Execute(new CurrentCashgames.Request(CurrentUserName, bunchId));
        return new CurrentCashgameListModel(currentGamesResult, _urls);
    }

    [Route(ApiRoutes.Cashgame.YearsByBunch)]
    [HttpGet]
    [ApiAuthorize]
    public CashgameYearListModel Years(string bunchId)
    {
        var listResult = _cashgameYearList.Execute(new CashgameYearList.Request(CurrentUserName, bunchId));
        return new CashgameYearListModel(listResult, _urls);
    }
}