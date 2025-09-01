using System.Linq;
using Api.Models.LocationModels;
using Api.Routes;
using Api.Settings;
using Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class LocationController(
    AppSettings appSettings,
    GetLocation getLocation,
    GetLocationList getLocationList,
    AddLocation addLocation)
    : BaseController(appSettings)
{
    /// <summary>
    /// Get a location
    /// </summary>
    [Route(ApiRoutes.Location.Get)]
    [HttpGet]
    [Authorize]
    public async Task<ObjectResult> Get(string locationId)
    {
        var result = await getLocation.Execute(new GetLocation.Request(Principal, locationId));
        return Model(result, () => result.Data is not null ? new LocationModel(result.Data) : null);
    }

    /// <summary>
    /// List locations
    /// </summary>
    [Route(ApiRoutes.Location.ListByBunch)]
    [HttpGet]
    [Authorize]
    public async Task<ObjectResult> GetList(string bunchId)
    {
        var result = await getLocationList.Execute(new GetLocationList.Request(Principal, bunchId));
        return Model(result, () => result.Data?.Locations.Select(o => new LocationModel(o)));
    }

    /// <summary>
    /// Add a location
    /// </summary>
    [Route(ApiRoutes.Location.Add)]
    [HttpPost]
    [Authorize]
    public async Task<ObjectResult> Add(string bunchId, [FromBody] LocationAddPostModel post)
    {
        var result = await addLocation.Execute(new AddLocation.Request(Principal, bunchId, post.Name));
        return Model(result, () => result.Data is not null ? new LocationModel(result.Data) : null);
    }
}