using Api.Extensions;
using Api.Models.BunchModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Handlers;

public static class UpdateBunchHandler
{
    public static async Task<IResult> Handle(
        EditBunch editBunch,
        IAuth auth,
        string bunchId,
        [FromBody] UpdateBunchPostModel post)
    {
        var request = new EditBunch.Request(auth, bunchId, post.Description, post.CurrencySymbol, post.CurrencyLayout, post.Timezone, post.HouseRules, post.DefaultBuyin);
        var result = await editBunch.Execute(request);
        return ResultHandler.Model(result, CreateModel);
        BunchModel? CreateModel() => result.Data is not null ? new BunchModel(result.Data) : null;
    }
}