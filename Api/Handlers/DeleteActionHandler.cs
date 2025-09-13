using Api.Auth;
using Api.Extensions;
using Api.Models.CommonModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class DeleteActionHandler
{
    public static async Task<IResult> Handle(
        DeleteCheckpoint deleteCheckpoint, 
        IAuth auth, 
        string cashgameId, 
        string actionId)
    {
        var result = await deleteCheckpoint.Execute(new DeleteCheckpoint.Request(auth, actionId));
        return ResultHandler.Model(result, () => new OkModel());
    }
}