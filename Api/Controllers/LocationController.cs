using Api.Auth;
using Api.Models.LocationModels;
using Api.Routes;
using Api.Settings;
using Core.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class LocationController : BaseController
    {
        public LocationController(AppSettings appSettings) : base(appSettings)
        {
        }

        [Route(ApiRoutes.Location.Get)]
        [HttpGet]
        [ApiAuthorize]
        public LocationModel Get(int locationId)
        {
            var result = UseCase.GetLocation.Execute(new GetLocation.Request(CurrentUserName, locationId));
            return new LocationModel(result);
        }

        [Route(ApiRoutes.Location.ListByBunch)]
        [HttpGet]
        [ApiAuthorize]
        public LocationListModel GetList(string bunchId)
        {
            var locationListResult = UseCase.GetLocationList.Execute(new GetLocationList.Request(CurrentUserName, bunchId));
            return new LocationListModel(locationListResult);
        }

        [Route(ApiRoutes.Location.ListByBunch)]
        [HttpPost]
        [ApiAuthorize]
        public LocationModel Add(string bunchId, [FromBody] LocationAddPostModel post)
        {
            var result = UseCase.AddLocation.Execute(new AddLocation.Request(CurrentUserName, bunchId, post.Name));
            return new LocationModel(result);
        }
    }
}