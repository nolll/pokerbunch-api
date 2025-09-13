using System.Linq;
using Api.Auth;
using Api.Extensions;
using Api.Models.BunchModels;
using Api.Models.CashgameModels;
using Api.Urls.ApiUrls;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Handlers;

public static class AddBunchHandler
{
    public static async Task<IResult> Handle(AddBunch addBunch, IAuth auth, [FromBody] AddBunchPostModel post)
    {
        var request = new AddBunch.Request(auth.Principal.UserName, post.Name, post.Description, post.CurrencySymbol, post.CurrencyLayout, post.Timezone);
        var result = await addBunch.Execute(request);
        return ResultHandler.Model(result, CreateModel);
        BunchModel? CreateModel() => result.Data is not null ? new BunchModel(result.Data) : null;
    }
}

public static class GetCurrentCashgamesHandler
{
    public static async Task<IResult> Handle(CurrentCashgames currentCashgames, IAuth auth, UrlProvider urls, string bunchId)
    {
        var result = await currentCashgames.Execute(new CurrentCashgames.Request(auth.Principal, bunchId));
        return ResultHandler.Model(result, () => result.Data?.Games.Select(o => new ApiCurrentGame(o, urls)));
    }
}
