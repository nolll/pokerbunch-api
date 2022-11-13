using System.Collections.Generic;
using System.Linq;
using Core.UseCases;

namespace Api.Models.LocationModels;

public class LocationListModel : List<LocationModel>
{
    public LocationListModel(GetLocationList.Result locationListResult)
    {
        AddRange(locationListResult.Locations.Select(o => new LocationModel(o)));
    }
}