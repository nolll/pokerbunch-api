using System.Linq;
using Api.Auth;
using Api.Models.BunchModels;
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
    private readonly GetBunchListForUser _getBunchListForUser;
    private readonly AddBunch _addBunch;
    private readonly JoinBunch _joinBunch;

    public BunchController(
        AppSettings appSettings, 
        GetBunch getBunch, 
        EditBunch editBunch,
        GetBunchList getBunchList,
        GetBunchListForUser getBunchListForUser,
        AddBunch addBunch,
        JoinBunch joinBunch)
        : base(appSettings)
    {
        _getBunch = getBunch;
        _editBunch = editBunch;
        _getBunchList = getBunchList;
        _getBunchListForUser = getBunchListForUser;
        _addBunch = addBunch;
        _joinBunch = joinBunch;
    }

    [Route(ApiRoutes.Bunch.Get)]
    [HttpGet]
    [ApiAuthorize]
    public async Task<ObjectResult> Get(string bunchId)
    {
        var request = new GetBunch.Request(CurrentUserName, bunchId);
        var result = await _getBunch.Execute(request);
        return Model(result, () => new BunchModel(result.Data));
    }

    [Route(ApiRoutes.Bunch.Update)]
    [HttpPut]
    [ApiAuthorize]
    public async Task<ObjectResult> Update(string bunchId, [FromBody] UpdateBunchPostModel post)
    {
        var request = new EditBunch.Request(CurrentUserName, bunchId, post.Description, post.CurrencySymbol, post.CurrencyLayout, post.Timezone, post.HouseRules, post.DefaultBuyin);
        var result = await _editBunch.Execute(request);
        return Model(result, () => new BunchModel(result.Data));
    }

    [Route(ApiRoutes.Bunch.List)]
    [HttpGet]
    [ApiAuthorize]
    public async Task<ObjectResult> List()
    {
        var request = new GetBunchList.Request(CurrentUserName);
        var result = await _getBunchList.Execute(request);
        return Model(result, () => result.Data.Bunches.Select(o => new BunchModel(o)));
    }

    [Route(ApiRoutes.Bunch.ListForCurrentUser)]
    [HttpGet]
    [ApiAuthorize]
    public async Task<ObjectResult> Bunches()
    {
        var result = await _getBunchListForUser.Execute(new GetBunchListForUser.Request(CurrentUserName));
        return Model(result, () => result.Data.Bunches.Select(o => new BunchModel(o)));
    }

    [Route(ApiRoutes.Bunch.Add)]
    [HttpPost]
    [ApiAuthorize]
    public async Task<ObjectResult> Add([FromBody] AddBunchPostModel post)
    {
        var request = new AddBunch.Request(CurrentUserName, post.Name, post.Description, post.CurrencySymbol, post.CurrencyLayout, post.Timezone);
        var result = await _addBunch.Execute(request);
        return Model(result, () => new BunchModel(result.Data));
    }

    [Route(ApiRoutes.Bunch.Join)]
    [HttpPost]
    [ApiAuthorize]
    public async Task<ObjectResult> Join(string bunchId, [FromBody] JoinBunchPostModel post)
    {
        var request = new JoinBunch.Request(CurrentUserName, bunchId, post.Code);
        var result = await _joinBunch.Execute(request);
        return Model(result, () => new PlayerJoinedModel(result.Data.PlayerId));
    }
}