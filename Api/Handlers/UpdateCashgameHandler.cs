using Api.Auth;
using Api.Extensions;
using Api.Models.CashgameModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Handlers;

public static class UpdateCashgameHandler
{
    public static async Task<IResult> Handle(EditCashgame editCashgame, CashgameDetails cashgameDetails, IAuth auth, string cashgameId, [FromBody] UpdateCashgamePostModel post)
    {
        var updateRequest = new EditCashgame.Request(auth, cashgameId, post.LocationId, post.EventId);
        var updateResult = await editCashgame.Execute(updateRequest);
        if(!updateResult.Success)
            return ResultHandler.Error(updateResult.Error);

        var detailsRequest = new CashgameDetails.Request(auth, cashgameId, DateTime.UtcNow);
        var detailsResult = await cashgameDetails.Execute(detailsRequest);
        return ResultHandler.Model(detailsResult, () => detailsResult.Data is not null ? new CashgameDetailsModel(detailsResult.Data!) : null);
    }
}