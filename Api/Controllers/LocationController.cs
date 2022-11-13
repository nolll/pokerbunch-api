using System.Collections.Generic;
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
    public LocationModel Get(int locationId)
    {
        var result = _getLocation.Execute(new GetLocation.Request(CurrentUserName, locationId));
        return new LocationModel(result);
    }

    [Route(ApiRoutes.Location.ListByBunch)]
    [HttpGet]
    [ApiAuthorize]
    public IEnumerable<LocationModel> GetList(string bunchId)
    {
        var locationListResult = _getLocationList.Execute(new GetLocationList.Request(CurrentUserName, bunchId));
        return locationListResult.Locations.Select(o => new LocationModel(o));
    }

    [Route(ApiRoutes.Location.Add)]
    [HttpPost]
    [ApiAuthorize]
    public LocationModel Add(string bunchId, [FromBody] LocationAddPostModel post)
    {
        var result = _addLocation.Execute(new AddLocation.Request(CurrentUserName, bunchId, post.Name));
        return new LocationModel(result);
    }
}