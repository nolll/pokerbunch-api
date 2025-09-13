using Api.Extensions;
using Api.Models.CommonModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Endpoints;

public static class ClearCacheHandler
{
    public static async Task<IResult> Handle(ClearCache clearCache, IAuth auth)
    {
        var result = await clearCache.Execute(new ClearCache.Request(auth));
        return ResultHandler.Model(result, () => new MessageModel(result.Data?.Message));
    }
}