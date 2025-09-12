using Api.Auth;
using Api.Extensions;
using Api.Models.BunchModels;
using Api.Routes;
using Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class GetBunchHandler
{
    public static async Task<IResult> Handle(GetBunch getBunch, IAuth auth, string bunchId)
    {
        var request = new GetBunch.Request(auth.Principal, bunchId);
        var result = await getBunch.Execute(request);
        return ResultHandler.Model(result, CreateModel);
        BunchModel? CreateModel() => result.Data is not null ? new BunchModel(result.Data) : null;
    }
}