using System.Collections.Generic;
using System.Linq;
using Api.Models.BunchModels;
using Api.Models.PlayerModels;
using Api.Routes;
using Api.Settings;
using Core.UseCases;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class BunchController(
    AppSettings appSettings,
    GetBunch getBunch,
    EditBunch editBunch,
    GetBunchList getBunchList,
    GetBunchListForUser getBunchListForUser,
    AddBunch addBunch,
    JoinBunch joinBunch)
    : BaseController(appSettings)
{
    [Route(ApiRoutes.Bunch.Get)]
    [HttpGet]
    [Authorize]
    [EndpointSummary("Get bunch")]
    public async Task<ObjectResult> Get(string bunchId)
    {
        var request = new GetBunch.Request(Principal, bunchId);
        var result = await getBunch.Execute(request);
        BunchModel? CreateModel() => result.Data is not null ? new BunchModel(result.Data) : null;
        return Model(result, CreateModel);
    }
    
    [Route(ApiRoutes.Bunch.Update)]
    [HttpPut]
    [Authorize]
    [EndpointSummary("Update bunch")]
    public async Task<ObjectResult> Update(string bunchId, [FromBody] UpdateBunchPostModel post)
    {
        var request = new EditBunch.Request(Principal, bunchId, post.Description, post.CurrencySymbol, post.CurrencyLayout, post.Timezone, post.HouseRules, post.DefaultBuyin);
        var result = await editBunch.Execute(request);
        BunchModel? CreateModel() => result.Data is not null ? new BunchModel(result.Data) : null;
        return Model(result, CreateModel);
    }
    
    [EndpointSummary("List bunches")]
    [Route(ApiRoutes.Bunch.List)]
    [HttpGet]
    [Authorize]
    public async Task<ObjectResult> List()
    {
        var request = new GetBunchList.Request(Principal);
        var result = await getBunchList.Execute(request);
        return Model(result, CreateModel);
        IEnumerable<BunchModel>? CreateModel() => result.Data?.Bunches.Select(o => new BunchModel(o));
    }
    
    [Route(ApiRoutes.Bunch.ListForCurrentUser)]
    [HttpGet]
    [Authorize]
    [EndpointSummary("List your bunches")]
    public async Task<ObjectResult> Bunches()
    {
        var result = await getBunchListForUser.Execute(new GetBunchListForUser.Request(CurrentUserName));
        return Model(result, CreateModel);
        IEnumerable<BunchModel>? CreateModel() => result.Data?.Bunches.Select(o => new BunchModel(o));
    }
    
    [Route(ApiRoutes.Bunch.Add)]
    [HttpPost]
    [Authorize]
    [EndpointSummary("Add bunch")]
    public async Task<ObjectResult> Add([FromBody] AddBunchPostModel post)
    {
        var request = new AddBunch.Request(CurrentUserName, post.Name, post.Description, post.CurrencySymbol, post.CurrencyLayout, post.Timezone);
        var result = await addBunch.Execute(request);
        return Model(result, CreateModel);
        BunchModel? CreateModel() => result.Data is not null ? new BunchModel(result.Data) : null;
    }
    
    [Route(ApiRoutes.Bunch.Join)]
    [HttpPost]
    [Authorize]
    [EndpointSummary("Join bunch")]
    public async Task<ObjectResult> Join(string bunchId, [FromBody] JoinBunchPostModel post)
    {
        var request = new JoinBunch.Request(CurrentUserName, bunchId, post.Code);
        var result = await joinBunch.Execute(request);
        return Model(result, CreateModel);
        PlayerJoinedModel? CreateModel() => result.Data?.PlayerId is not null ? new PlayerJoinedModel(result.Data.PlayerId) : null;
    }
}