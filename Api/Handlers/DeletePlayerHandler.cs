using Api.Extensions;
using Api.Models.PlayerModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class DeletePlayerHandler
{
    public static async Task<IResult> Handle(DeletePlayer deletePlayer, IAuth auth, string playerId)
    {
        var request = new DeletePlayer.Request(auth, playerId);
        var result = await deletePlayer.Execute(request);
        return ResultHandler.Model(result, () => new PlayerDeletedModel(playerId));
    }
}