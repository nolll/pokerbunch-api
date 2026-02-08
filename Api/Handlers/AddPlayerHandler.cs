using Api.Extensions;
using Api.Models.PlayerModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Handlers;

public static class AddPlayerHandler
{
    public static async Task<IResult> Handle(
        AddPlayer addPlayer, 
        GetPlayer getPlayer, 
        IAuth auth,
        string bunchId,
        [FromBody] PlayerAddPostModel post)
    {
        var result = await addPlayer.Execute(new AddPlayer.Request(auth, bunchId, post.Name));
        return result.Success 
            ? await GetPlayerHandler.Handle(getPlayer, auth, result.Data?.Id ?? "")
            : ResultHandler.Error(result.Error);
    }
}