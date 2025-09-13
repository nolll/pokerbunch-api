using Api.Auth;
using Api.Extensions;
using Api.Models.CashgameModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class GetCashgameHandler
{
    public static async Task<IResult> Handle(CashgameDetails cashgameDetails, IAuth auth, string cashgameId)
    {
        var request = new CashgameDetails.Request(auth, cashgameId, DateTime.UtcNow);
        var result = await cashgameDetails.Execute(request);
        return ResultHandler.Model(result, CreateModel);
        CashgameDetailsModel? CreateModel() => result.Data is not null ? new CashgameDetailsModel(result.Data) : null;
    }
}