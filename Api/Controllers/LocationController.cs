using System.Linq;
using Api.Models.LocationModels;
using Api.Routes;
using Api.Settings;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class LocationController(
    AppSettings appSettings,
    GetLocation getLocation,
    GetLocationList getLocationList,
    AddLocation addLocation,
    IAuth auth)
    : BaseController(appSettings)
{
    [Route(ApiRoutes.Location.Get)]
    [HttpGet]
    [Authorize]
    [EndpointSummary("Get location")]
    public async Task<ObjectResult> Get(string locationId)
    {
        var result = await getLocation.Execute(new GetLocation.Request(auth, locationId));
        return Model(result, () => result.Data is not null ? new LocationModel(result.Data) : null);
    }
    
    [Route(ApiRoutes.Location.ListByBunch)]
    [HttpGet]
    [Authorize]
    [EndpointSummary("List locations")]
    public async Task<ObjectResult> GetList(string bunchId)
    {
        var result = await getLocationList.Execute(new GetLocationList.Request(auth, bunchId));
        return Model(result, () => result.Data?.Locations.Select(o => new LocationModel(o)));
    }
    
    [Route(ApiRoutes.Location.Add)]
    [HttpPost]
    [Authorize]
    [EndpointSummary("Add location")]
    public async Task<ObjectResult> Add(string bunchId, [FromBody] LocationAddPostModel post)
    {
        var result = await addLocation.Execute(new AddLocation.Request(auth, bunchId, post.Name));
        return Model(result, () => result.Data is not null ? new LocationModel(result.Data) : null);
    }
}