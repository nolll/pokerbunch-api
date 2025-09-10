using System.Linq;
using Api.Models.PlayerModels;
using Api.Routes;
using Api.Settings;
using Api.Urls.ApiUrls;
using Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class PlayerController(
    AppSettings appSettings,
    UrlProvider urls,
    GetPlayer getPlayer,
    GetPlayerList getPlayerList,
    AddPlayer addPlayer,
    DeletePlayer deletePlayer,
    InvitePlayer invitePlayer)
    : BaseController(appSettings)
{
    [Route(ApiRoutes.Player.Get)]
    [HttpGet]
    [Authorize]
    [EndpointSummary("Get player")]
    public async Task<ObjectResult> Get(string playerId)
    {
        var result = await getPlayer.Execute(new GetPlayer.Request(Principal, playerId));
        return Model(result, () => result.Data is not null ? new PlayerModel(result.Data) : null);
    }
    
    [Route(ApiRoutes.Player.ListByBunch)]
    [HttpGet]
    [Authorize]
    [EndpointSummary("List bunch players")]
    public async Task<ObjectResult> GetList(string bunchId)
    {
        var result = await getPlayerList.Execute(new GetPlayerList.Request(Principal, bunchId));
        return Model(result, () => result.Data?.Players.Select(o => new PlayerListItemModel(o)));
    }
    
    [Route(ApiRoutes.Player.Add)]
    [HttpPost]
    [Authorize]
    [EndpointSummary("Add player to bunch")]
    public async Task<ObjectResult> Add(string bunchId, [FromBody] PlayerAddPostModel post)
    {
        var result = await addPlayer.Execute(new AddPlayer.Request(Principal, bunchId, post.Name));
        return result.Success 
            ? await Get(result.Data?.Id ?? "")
            : Error(result.Error);
    }
    
    [Route(ApiRoutes.Player.Delete)]
    [HttpDelete]
    [Authorize]
    [EndpointSummary("Delete player from bunch")]
    public async Task<ObjectResult> Delete(string playerId)
    {
        var request = new DeletePlayer.Request(Principal, playerId);
        var result = await deletePlayer.Execute(request);
        return Model(result, () => new PlayerDeletedModel(playerId));
    }
    
    [Route(ApiRoutes.Player.Invite)]
    [HttpPost]
    [Authorize]
    [EndpointSummary("Invite player to bunch")]
    public async Task<ObjectResult> Invite(string playerId, [FromBody] PlayerInvitePostModel post)
    {
        var registerUrl = urls.Site.AddUser;
        var joinBunchUrlFormat = urls.Site.JoinBunch("{0}");
        var joinBunchWithCodeUrlFormat = urls.Site.JoinBunch("{0}", "{1}");
        var request = new InvitePlayer.Request(Principal, playerId, post.Email, registerUrl, joinBunchUrlFormat, joinBunchWithCodeUrlFormat);
        var result = await invitePlayer.Execute(request);
        return Model(result, () => new PlayerInvitedModel(playerId));
    }
}