using System.Linq;
using Api.Auth;
using Api.Models.PlayerModels;
using Api.Routes;
using Api.Settings;
using Api.Urls.ApiUrls;
using Core.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class PlayerController : BaseController
{
    private readonly UrlProvider _urls;
    private readonly GetPlayer _getPlayer;
    private readonly GetPlayerList _getPlayerList;
    private readonly AddPlayer _addPlayer;
    private readonly DeletePlayer _deletePlayer;
    private readonly InvitePlayer _invitePlayer;

    public PlayerController(
        AppSettings appSettings, 
        UrlProvider urls,
        GetPlayer getPlayer,
        GetPlayerList getPlayerList,
        AddPlayer addPlayer,
        DeletePlayer deletePlayer,
        InvitePlayer invitePlayer) : base(appSettings)
    {
        _urls = urls;
        _getPlayer = getPlayer;
        _getPlayerList = getPlayerList;
        _addPlayer = addPlayer;
        _deletePlayer = deletePlayer;
        _invitePlayer = invitePlayer;
    }

    /// <summary>
    /// Gets a specific player.
    /// </summary>
    [Route(ApiRoutes.Player.Get)]
    [HttpGet]
    [ApiAuthorize]
    public ObjectResult Get(int playerId)
    {
        var result = _getPlayer.Execute(new GetPlayer.Request(CurrentUserName, playerId));
        return Model(result, () => new PlayerModel(result.Data));
    }

    /// <summary>
    /// Lists all players in a bunch.
    /// </summary>
    [Route(ApiRoutes.Player.ListByBunch)]
    [HttpGet]
    [ApiAuthorize]
    public ObjectResult GetList(string bunchId)
    {
        var result = _getPlayerList.Execute(new GetPlayerList.Request(CurrentUserName, bunchId));
        return Model(result, () => result.Data.Players.Select(o => new PlayerListItemModel(o)));
    }

    /// <summary>
    /// Adds a player to a bunch.
    /// </summary>
    [Route(ApiRoutes.Player.ListByBunch)]
    [HttpPost]
    [ApiAuthorize]
    public ObjectResult Add(string bunchId, [FromBody] PlayerAddPostModel post)
    {
        var result = _addPlayer.Execute(new AddPlayer.Request(CurrentUserName, bunchId, post.Name));
        return result.Success 
            ? Get(result.Data.Id) 
            : Error(result.Error);
    }

    /// <summary>
    /// Deletes a specific player.
    /// </summary>
    [Route(ApiRoutes.Player.Get)]
    [HttpDelete]
    [ApiAuthorize]
    public ObjectResult Delete(int playerId)
    {
        var request = new DeletePlayer.Request(CurrentUserName, playerId);
        var result = _deletePlayer.Execute(request);
        return Model(result, () => new PlayerDeletedModel(playerId));
    }

    /// <summary>
    /// Invites a player to a bunch.
    /// </summary>
    [Route(ApiRoutes.Player.Invite)]
    [HttpPost]
    [ApiAuthorize]
    public ObjectResult Invite(int playerId, [FromBody] PlayerInvitePostModel post)
    {
        var registerUrl = _urls.Site.AddUser;
        var joinBunchUrlFormat = _urls.Site.JoinBunch("{0}");
        var joinBunchWithCodeUrlFormat = _urls.Site.JoinBunch("{0}", "{1}");
        var request = new InvitePlayer.Request(CurrentUserName, playerId, post.Email, registerUrl, joinBunchUrlFormat, joinBunchWithCodeUrlFormat);
        var result = _invitePlayer.Execute(request);
        return Model(result, () => new PlayerInvitedModel(playerId));
    }
}