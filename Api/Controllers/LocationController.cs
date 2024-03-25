using System.Linq;
using Api.Models.LocationModels;
using Api.Routes;
using Api.Settings;
using Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class LocationController : BaseController
{
    private readonly GetLocation _getLocation;
    private readonly GetLocationList _getLocationList;
    private readonly AddLocation _addLocation;

    public LocationController(
        AppSettings appSettings,
        GetLocation getLocation,
        GetLocationList getLocationList,
        AddLocation addLocation) : base(appSettings)
    {
        _getLocation = getLocation;
        _getLocationList = getLocationList;
        _addLocation = addLocation;
    }

    /// <summary>
    /// Get a location
    /// </summary>
    [Route(ApiRoutes.Location.Get)]
    [HttpGet]
    [Authorize]
    public async Task<ObjectResult> Get(string locationId)
    {
        var result = await _getLocation.Execute(new GetLocation.Request(CurrentUserName, locationId));
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
        var result = await _getLocationList.Execute(new GetLocationList.Request(CurrentUserName, bunchId));
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
        var result = await _addLocation.Execute(new AddLocation.Request(CurrentUserName, bunchId, post.Name));
        return Model(result, () => result.Data is not null ? new LocationModel(result.Data) : null);
    }
}