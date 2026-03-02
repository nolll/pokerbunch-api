using System.Linq;
using Api.Extensions;
using Api.Models.CashgameModels;
using Core.Services;
using Core.Services.Interfaces;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class GetCurrentCashgamesHandler
{
    public static async Task<IResult> Handle(CurrentCashgames currentCashgames, IAuth auth, IApiUrlProvider apiUrlProvider, string bunchId)
    {
        var result = await currentCashgames.Execute(new CurrentCashgames.Request(auth, bunchId));
        return ResultHandler.Model(result, () => result.Data?.Games.Select(o => new ApiCurrentGame(o, apiUrlProvider)));
    }
}