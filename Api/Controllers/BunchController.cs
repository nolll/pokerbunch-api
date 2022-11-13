using System.Collections.Generic;
using System.Linq;
using Api.Auth;
using Api.Models.BunchModels;
using Api.Models.CommonModels;
using Api.Models.PlayerModels;
using Api.Routes;
using Api.Settings;
using Core.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class BunchController : BaseController
{
    private readonly GetBunch _getBunch;
    private readonly EditBunch _editBunch;
    private readonly GetBunchList _getBunchList;
    private readonly AddBunch _addBunch;
    private readonly JoinBunch _joinBunch;

    public BunchController(
        AppSettings appSettings, 
        GetBunch getBunch, 
        EditBunch editBunch, 
        GetBunchList getBunchList,
        AddBunch addBunch,
        JoinBunch joinBunch)
        : base(appSettings)
    {
        _getBunch = getBunch;
        _editBunch = editBunch;
        _getBunchList = getBunchList;
        _addBunch = addBunch;
        _joinBunch = joinBunch;
    }

    [Route(ApiRoutes.Bunch.Get)]
    [HttpGet]
    [ApiAuthorize]
    public BunchModel Get(string bunchId)
    {
        var request = new GetBunch.Request(CurrentUserName, bunchId);
        var bunchResult = _getBunch.Execute(request);
        return new BunchModel(bunchResult);
    }

    [Route(ApiRoutes.Bunch.Get)]
    [HttpPut]
    [ApiAuthorize]
    public BunchModel Update(string bunchId, [FromBody] UpdateBunchPostModel post)
    {
        var request = new EditBunch.Request(CurrentUserName, bunchId, post.Description, post.CurrencySymbol, post.CurrencyLayout, post.Timezone, post.HouseRules, post.DefaultBuyin);
        var bunchResult = _editBunch.Execute(request);
        return new BunchModel(bunchResult);
    }

    [Route(ApiRoutes.Bunch.List)]
    [HttpGet]
    [ApiAuthorize]
    public IEnumerable<BunchModel> List()
    {
        var request = new GetBunchList.AllBunchesRequest(CurrentUserName);
        var bunchListResult = _getBunchList.Execute(request);
        return bunchListResult.Bunches.Select(o => new BunchModel(o));
    }

    [Route(ApiRoutes.Bunch.ListForCurrentUser)]
    [HttpGet]
    [ApiAuthorize]
    public IEnumerable<BunchModel> Bunches()
    {
        var bunchListResult = _getBunchList.Execute(new GetBunchList.UserBunchesRequest(CurrentUserName));
        return bunchListResult.Bunches.Select(o => new BunchModel(o));
    }

    [Route(ApiRoutes.Bunch.List)]
    [HttpPost]
    [ApiAuthorize]
    public BunchModel Add([FromBody] AddBunchPostModel post)
    {
        var request = new AddBunch.Request(CurrentUserName, post.Name, post.Description, post.CurrencySymbol, post.CurrencyLayout, post.Timezone);
        var bunchResult = _addBunch.Execute(request);
        return new BunchModel(bunchResult);
    }

    [Route(ApiRoutes.Bunch.Join)]
    [HttpPost]
    [ApiAuthorize]
    public MessageModel Join(string bunchId, [FromBody] JoinBunchPostModel post)
    {
        var request = new JoinBunch.Request(CurrentUserName, bunchId, post.Code);
        var result = _joinBunch.Execute(request);
        return new PlayerJoinedModel(result.PlayerId);
    }
}