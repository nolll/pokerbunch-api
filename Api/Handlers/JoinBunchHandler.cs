using Api.Auth;
using Api.Extensions;
using Api.Models.BunchModels;
using Api.Models.PlayerModels;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Handlers;

public static class JoinBunchHandler
{
    public static async Task<IResult> Handle(JoinBunch joinBunch, IAuth auth, string bunchId, [FromBody] JoinBunchPostModel post)
    {
        var request = new JoinBunch.Request(auth.Principal.UserName, bunchId, post.Code);
        var result = await joinBunch.Execute(request);
        return ResultHandler.Model(result, CreateModel);
        PlayerJoinedModel? CreateModel() => result.Data?.PlayerId is not null ? new PlayerJoinedModel(result.Data.PlayerId) : null;
    }
}