using Api.Extensions;
using Api.Models.PlayerModels;
using Api.Urls.ApiUrls;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class InvitePlayerHandler
{
    public static async Task<IResult> Handle(
        InvitePlayer invitePlayer,
        IAuth auth,
        UrlProvider urls,
        string playerId,
        [FromBody] PlayerInvitePostModel post)
    {
        var registerUrl = urls.Site.AddUser;
        var joinBunchUrlFormat = urls.Site.JoinBunch("{0}");
        var joinBunchWithCodeUrlFormat = urls.Site.JoinBunch("{0}", "{1}");
        var request = new InvitePlayer.Request(auth, playerId, post.Email, registerUrl, joinBunchUrlFormat, joinBunchWithCodeUrlFormat);
        var result = await invitePlayer.Execute(request);
        return ResultHandler.Model(result, () => new PlayerInvitedModel(playerId));
    }
}