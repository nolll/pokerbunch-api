using Api.Extensions;
using Api.Models.CashgameModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Handlers;

public static class AddCashgameHandler
{
    public static async Task<IResult> Handle(AddCashgame addCashgame, CashgameDetails cashgameDetails, IAuth auth, string bunchId, [FromBody] AddCashgamePostModel post)
    {
        var addRequest = new AddCashgame.Request(auth, bunchId, post.LocationId);
        var addResult = await addCashgame.Execute(addRequest);
        if (!addResult.Success)
            return ResultHandler.Error(addResult.Error);

        var detailsRequest = new CashgameDetails.Request(auth, addResult.Data?.CashgameId ?? "", DateTime.UtcNow);
        var detailsResult = await cashgameDetails.Execute(detailsRequest);
        return ResultHandler.Model(detailsResult, () => detailsResult.Data is not null ? new CashgameDetailsModel(detailsResult.Data!) : null);
    }
}