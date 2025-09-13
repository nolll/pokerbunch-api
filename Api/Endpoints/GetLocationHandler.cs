using Api.Extensions;
using Api.Models.LocationModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Endpoints;

public static class GetLocationHandler
{
    public static async Task<IResult> Handle(GetLocation getLocation, IAuth auth, string locationId)
    {
        var result = await getLocation.Execute(new GetLocation.Request(auth, locationId));
        return ResultHandler.Model(result, () => result.Data is not null ? new LocationModel(result.Data) : null);
    }
}