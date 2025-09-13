using System.Collections.Generic;
using System.Linq;
using Api.Extensions;
using Api.Models.BunchModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class GetBunchListHandler
{
    public static async Task<IResult> Handle(
        GetBunchList getBunchList,
        IAuth auth)
    {
        var request = new GetBunchList.Request(auth);
        var result = await getBunchList.Execute(request);
        return ResultHandler.Model(result, CreateModel);
        IEnumerable<BunchModel>? CreateModel() => result.Data?.Bunches.Select(o => new BunchModel(o));
    }
}