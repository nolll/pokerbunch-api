using Api.Extensions;
using Api.Models.PlayerModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Endpoints;

public static class GetPlayerHandler
{
    public static async Task<IResult> Handle(GetPlayer getPlayer, IAuth auth, string playerId)
    {
        var result = await getPlayer.Execute(new GetPlayer.Request(auth, playerId));
        return ResultHandler.Model(result, () => result.Data is not null ? new PlayerModel(result.Data) : null);
    }
}