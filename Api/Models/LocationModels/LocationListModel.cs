using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models.LocationModels
{
    [CollectionDataContract(Namespace = "", Name = "locations", ItemName = "location")]
    public class LocationListModel : List<LocationModel>
    {
        public LocationListModel(GetLocationList.Result locationListResult)
        {
            AddRange(locationListResult.Locations.Select(o => new LocationModel(o)));
        }

        public LocationListModel()
        {
        }
    }
}