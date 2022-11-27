﻿using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiLocationUrl : ApiUrl
{
    private readonly string _locationId;

    public ApiLocationUrl(string locationId)
    {
        _locationId = locationId;
    }

    public ApiLocationUrl(int locationId)
        : this(locationId.ToString())
    {
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.Location.Get, RouteReplace.LocationId(_locationId));
}