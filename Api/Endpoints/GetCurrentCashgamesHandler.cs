using System.Linq;
using Api.Extensions;
using Api.Models.CashgameModels;
using Api.Urls.ApiUrls;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Endpoints;

public static class GetCurrentCashgamesHandler
{
    public static async Task<IResult> Handle(CurrentCashgames currentCashgames, IAuth auth, UrlProvider urls, string bunchId)
    {
        var result = await currentCashgames.Execute(new CurrentCashgames.Request(auth, bunchId));
        return ResultHandler.Model(result, () => result.Data?.Games.Select(o => new ApiCurrentGame(o, urls)));
    }
}