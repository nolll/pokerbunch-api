using Api.Auth;
using Api.Models.LocationModels;
using Api.Routes;
using Api.Settings;
using Core.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class TimezoneController : BaseController
    {
        private readonly GetTimezoneList _getTimezoneList;

        public TimezoneController(
            AppSettings appSettings,
            GetTimezoneList getTimezoneList)
            : base(appSettings)
        {
            _getTimezoneList = getTimezoneList;
        }

        [Route(ApiRoutes.Location.ListByBunch)]
        [HttpGet]
        public TimezoneListModel GetList(string bunchId)
        {
            var timezoneListResult = _getTimezoneList.Execute();
            return new TimezoneListModel(timezoneListResult);
        }
    }

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
        public LocationListModel GetList(string bunchId)
        {
            var locationListResult = _getLocationList.Execute(new GetLocationList.Request(CurrentUserName, bunchId));
            return new LocationListModel(locationListResult);
        }

        [Route(ApiRoutes.Location.ListByBunch)]
        [HttpPost]
        [ApiAuthorize]
        public LocationModel Add(string bunchId, [FromBody] LocationAddPostModel post)
        {
            var result = _addLocation.Execute(new AddLocation.Request(CurrentUserName, bunchId, post.Name));
            return new LocationModel(result);
        }
    }
}