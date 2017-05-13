using System.Web.Http;
using Api.Auth;
using Api.Models.LocationModels;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers
{
    public class LocationController : BaseController
    {
        [Route(ApiRoutes.LocationGet)]
        [HttpGet]
        [ApiAuthorize]
        public LocationModel Get(int id)
        {
            var result = UseCase.GetLocation.Execute(new GetLocation.Request(CurrentUserName, id));
            return new LocationModel(result);
        }

        [Route(ApiRoutes.LocationList)]
        [HttpGet]
        [ApiAuthorize]
        public LocationListModel GetList(string slug)
        {
            var locationListResult = UseCase.GetLocationList.Execute(new GetLocationList.Request(CurrentUserName, slug));
            return new LocationListModel(locationListResult);
        }

        [Route(ApiRoutes.LocationAdd)]
        [HttpPost]
        [ApiAuthorize]
        public LocationModel Add(string slug, [FromBody] LocationAddPostModel location)
        {
            var result = UseCase.AddLocation.Execute(new AddLocation.Request(CurrentUserName, slug, location.Name));
            return new LocationModel(result);
        }
    }
}