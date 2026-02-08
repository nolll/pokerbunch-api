using System.Linq;
using Api.Extensions;
using Api.Models.PlayerModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class GetPlayerListHandler
{
    public static async Task<IResult> Handle(GetPlayerList getPlayerList, IAuth auth, string bunchId)
    {
        var result = await getPlayerList.Execute(new GetPlayerList.Request(auth, bunchId));
        return ResultHandler.Model(result, () => result.Data?.Players.Select(o => new PlayerListItemModel(o)));
    }
}