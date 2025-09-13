using System.Linq;
using Api.Extensions;
using Api.Models.LocationModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class GetLocationListHandler
{
    public static async Task<IResult> Handle(GetLocationList getLocationList, IAuth auth, string bunchId)
    {
        var result = await getLocationList.Execute(new GetLocationList.Request(auth, bunchId));
        return ResultHandler.Model(result, () => result.Data?.Locations.Select(o => new LocationModel(o)));
    }
}