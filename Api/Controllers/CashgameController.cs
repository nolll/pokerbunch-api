using System;
using System.Collections.Generic;
using System.Linq;
using Api.Auth;
using Api.Models.CashgameModels;
using Api.Models.CommonModels;
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
    public ObjectResult Get(int cashgameId)
    {
        var request = new CashgameDetails.Request(CurrentUserName, cashgameId, DateTime.UtcNow);
        var result = _cashgameDetails.Execute(request);
        return Model(result, () => new CashgameDetailsModel(result.Data));
    }

    [Route(ApiRoutes.Cashgame.ListByBunch)]
    [HttpGet]
    [ApiAuthorize]
    public ObjectResult List(string bunchId)
    {
        var result = _cashgameList.Execute(new CashgameList.Request(CurrentUserName, bunchId, CashgameList.SortOrder.Date, null));
        return Model(result, () => result.Data.Items.Select(o => new CashgameListItemModel(o)));
    }

    [Route(ApiRoutes.Cashgame.ListByBunchAndYear)]
    [HttpGet]
    [ApiAuthorize]
    public ObjectResult List(string bunchId, int year)
    {
        var result = _cashgameList.Execute(new CashgameList.Request(CurrentUserName, bunchId, CashgameList.SortOrder.Date, year));
        return Model(result, () => result.Data.Items.Select(o => new CashgameListItemModel(o)));
    }

    [Route(ApiRoutes.Cashgame.ListByEvent)]
    [HttpGet]
    [ApiAuthorize]
    public ObjectResult EventCashgameList(int eventId)
    {
        var result = _eventCashgameList.Execute(new EventCashgameList.Request(CurrentUserName, eventId));
        return Model(result, () => result.Data.Items.Select(o => new CashgameListItemModel(o)));
    }

    [Route(ApiRoutes.Cashgame.ListByPlayer)]
    [HttpGet]
    [ApiAuthorize]
    public ObjectResult PlayerCashgameList(int playerId)
    {
        var result = _playerCashgameList.Execute(new PlayerCashgameList.Request(CurrentUserName, playerId));
        return Model(result, () => result.Data.Items.Select(o => new CashgameListItemModel(o)));
    }

    [Route(ApiRoutes.Cashgame.Add)]
    [HttpPost]
    [ApiAuthorize]
    public ObjectResult Add(string bunchId, [FromBody] AddCashgamePostModel post)
    {
        var addRequest = new AddCashgame.Request(CurrentUserName, bunchId, post.LocationId);
        var addResult = _addCashgame.Execute(addRequest);
        if (!addResult.Success)
            return Error(addResult.Error);

        var detailsRequest = new CashgameDetails.Request(CurrentUserName, addResult.Data.CashgameId, DateTime.UtcNow);
        var detailsResult = _cashgameDetails.Execute(detailsRequest);
        return Model(detailsResult, () => new CashgameDetailsModel(detailsResult.Data));
    }

    [Route(ApiRoutes.Cashgame.Update)]
    [HttpPut]
    [ApiAuthorize]
    public ObjectResult Update(int cashgameId, [FromBody] UpdateCashgamePostModel post)
    {
        var updateRequest = new EditCashgame.Request(CurrentUserName, cashgameId, post.LocationId, post.EventId);
        var updateResult = _editCashgame.Execute(updateRequest);
        if(!updateResult.Success)
            return Error(updateResult.Error);

        var detailsRequest = new CashgameDetails.Request(CurrentUserName, cashgameId, DateTime.UtcNow);
        var detailsResult = _cashgameDetails.Execute(detailsRequest);
        return Model(detailsResult, () => new CashgameDetailsModel(detailsResult.Data));
    }

    [Route(ApiRoutes.Cashgame.Delete)]
    [HttpDelete]
    [ApiAuthorize]
    public ObjectResult Delete(int cashgameId)
    {
        var request = new DeleteCashgame.Request(CurrentUserName, cashgameId);
        var result = _deleteCashgame.Execute(request);
        return Model(result, () => new CashgameDeletedModel(cashgameId));
    }

    [Route(ApiRoutes.Cashgame.ListCurrentByBunch)]
    [HttpGet]
    [ApiAuthorize]
    public ObjectResult Current(string bunchId)
    {
        var result = _currentCashgames.Execute(new CurrentCashgames.Request(CurrentUserName, bunchId));
        return Model(result, () => result.Data.Games.Select(o => new ApiCurrentGame(o, _urls)));
    }

    [Route(ApiRoutes.Cashgame.YearsByBunch)]
    [HttpGet]
    [ApiAuthorize]
    public ObjectResult Years(string bunchId)
    {
        var result = _cashgameYearList.Execute(new CashgameYearList.Request(CurrentUserName, bunchId));
        return Model(result, () => result.Data.Years.Select(o => new CashgameYearListItemModel(result.Data.Slug, o, _urls)));
    }
}