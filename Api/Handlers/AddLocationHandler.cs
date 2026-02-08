using Api.Extensions;
using Api.Models.LocationModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Handlers;

public static class AddLocationHandler
{
    public static async Task<IResult> Handle(
        AddLocation addLocation,
        IAuth auth,
        string bunchId,
        [FromBody] LocationAddPostModel post)
    {
        var result = await addLocation.Execute(new AddLocation.Request(auth, bunchId, post.Name));
        return ResultHandler.Model(result, () => result.Data is not null ? new LocationModel(result.Data) : null);
    }
}