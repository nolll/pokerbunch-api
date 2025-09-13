using Api.Extensions;
using Api.Models.BunchModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class GetBunchHandler
{
    public static async Task<IResult> Handle(GetBunch getBunch, IAuth auth, string bunchId)
    {
        var request = new GetBunch.Request(auth, bunchId);
        var result = await getBunch.Execute(request);
        return ResultHandler.Model(result, CreateModel);
        BunchModel? CreateModel() => result.Data is not null ? new BunchModel(result.Data) : null;
    }
}