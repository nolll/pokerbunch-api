using Api.Extensions;
using Api.Models.BunchModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Handlers;

public static class AddBunchHandler
{
    public static async Task<IResult> Handle(AddBunch addBunch, IAuth auth, [FromBody] AddBunchPostModel post)
    {
        var request = new AddBunch.Request(auth.UserName, post.Name, post.Description, post.CurrencySymbol, post.CurrencyLayout, post.Timezone);
        var result = await addBunch.Execute(request);
        return ResultHandler.Model(result, CreateModel);
        BunchModel? CreateModel() => result.Data is not null ? new BunchModel(result.Data) : null;
    }
}