using System.Linq;
using Api.Auth;
using Api.Models.LocationModels;
using Api.Routes;
using Api.Settings;
using Core.UseCases;
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

    [Route(ApiRoutes.Location.Get)]
    [HttpGet]
    [ApiAuthorize]
    public async Task<ObjectResult> Get(int locationId)
    {
        var result = await _getLocation.Execute(new GetLocation.Request(CurrentUserName, locationId));
        return Model(result, () => new LocationModel(result.Data));
    }

    [Route(ApiRoutes.Location.ListByBunch)]
    [HttpGet]
    [ApiAuthorize]
    public async Task<ObjectResult> GetList(string bunchId)
    {
        var result = await _getLocationList.Execute(new GetLocationList.Request(CurrentUserName, bunchId));
        return Model(result, () => result.Data.Locations.Select(o => new LocationModel(o)));
    }

    [Route(ApiRoutes.Location.Add)]
    [HttpPost]
    [ApiAuthorize]
    public async Task<ObjectResult> Add(string bunchId, [FromBody] LocationAddPostModel post)
    {
        var result = await _addLocation.Execute(new AddLocation.Request(CurrentUserName, bunchId, post.Name));
        return Model(result, () => new LocationModel(result.Data));
    }
}