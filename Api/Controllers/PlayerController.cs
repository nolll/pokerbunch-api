using System.Linq;
using Api.Models.PlayerModels;
using Api.Routes;
using Api.Settings;
using Api.Urls.ApiUrls;
using Core.UseCases;
using Microsoft.AspNetCore.Authorization;
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
    /// <summary>
    /// Get a player
    /// </summary>
    [Route(ApiRoutes.Player.Get)]
    [HttpGet]
    [Authorize]
    public async Task<ObjectResult> Get(string playerId)
    {
        var result = await getPlayer.Execute(new GetPlayer.Request(CurrentUserName, playerId));
        return Model(result, () => result.Data is not null ? new PlayerModel(result.Data) : null);
    }

    /// <summary>
    /// List all players in a bunch
    /// </summary>
    [Route(ApiRoutes.Player.ListByBunch)]
    [HttpGet]
    [Authorize]
    public async Task<ObjectResult> GetList(string bunchId)
    {
        var result = await getPlayerList.Execute(new GetPlayerList.Request(CurrentUserName, bunchId));
        return Model(result, () => result.Data?.Players.Select(o => new PlayerListItemModel(o)));
    }

    /// <summary>
    /// Add a player to a bunch
    /// </summary>
    [Route(ApiRoutes.Player.Add)]
    [HttpPost]
    [Authorize]
    public async Task<ObjectResult> Add(string bunchId, [FromBody] PlayerAddPostModel post)
    {
        var result = await addPlayer.Execute(new AddPlayer.Request(CurrentUserName, bunchId, post.Name));
        return result.Success 
            ? await Get(result.Data?.Id ?? "")
            : Error(result.Error);
    }

    /// <summary>
    /// Delete a player
    /// </summary>
    [Route(ApiRoutes.Player.Delete)]
    [HttpDelete]
    [Authorize]
    public async Task<ObjectResult> Delete(string playerId)
    {
        var request = new DeletePlayer.Request(CurrentUserName, playerId);
        var result = await deletePlayer.Execute(request);
        return Model(result, () => new PlayerDeletedModel(playerId));
    }

    /// <summary>
    /// Invite a player to a bunch
    /// </summary>
    [Route(ApiRoutes.Player.Invite)]
    [HttpPost]
    [Authorize]
    public async Task<ObjectResult> Invite(string playerId, [FromBody] PlayerInvitePostModel post)
    {
        var registerUrl = urls.Site.AddUser;
        var joinBunchUrlFormat = urls.Site.JoinBunch("{0}");
        var joinBunchWithCodeUrlFormat = urls.Site.JoinBunch("{0}", "{1}");
        var request = new InvitePlayer.Request(CurrentUserName, playerId, post.Email, registerUrl, joinBunchUrlFormat, joinBunchWithCodeUrlFormat);
        var result = await invitePlayer.Execute(request);
        return Model(result, () => new PlayerInvitedModel(playerId));
    }
}