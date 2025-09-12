using Api.Auth;
using Api.Extensions;
using Api.Models.CashgameModels;
using Api.Models.CommonModels;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Handlers;

public static class UpdateActionHandler
{
    public static async Task<IResult> Handle(
        EditCheckpoint editCheckpoint,
        IAuth auth,
        string cashgameId,
        string actionId,
        [FromBody] UpdateActionPostModel post)
    {
        var utcTimestamp = DateTime.SpecifyKind(post.Timestamp, DateTimeKind.Utc);
        var result = await editCheckpoint.Execute(new EditCheckpoint.Request(auth.Principal, actionId, utcTimestamp, post.Stack, post.Added));
        return ResultHandler.Model(result, () => new OkModel());
    }
}