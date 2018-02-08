using System.Web.Http;
using Api.Auth;
using Api.Models.LocationModels;
using Core.UseCases;
using PokerBunch.Common.Routes;

namespace Api.Controllers
{
    public class LocationController : BaseController
    {
        [Route(ApiRoutes.Location)]
        [HttpGet]
        [ApiAuthorize]
        public LocationModel Get(int locationId)
        {
            var result = UseCase.GetLocation.Execute(new GetLocation.Request(CurrentUserName, locationId));
            return new LocationModel(result);
        }

        [Route(ApiRoutes.LocationsByBunch)]
        [HttpGet]
        [ApiAuthorize]
        public LocationListModel GetList(string bunchId)
        {
            var locationListResult = UseCase.GetLocationList.Execute(new GetLocationList.Request(CurrentUserName, bunchId));
            return new LocationListModel(locationListResult);
        }

        [Route(ApiRoutes.LocationsByBunch)]
        [HttpPost]
        [ApiAuthorize]
        public LocationModel Add(string bunchId, [FromBody] LocationAddPostModel post)
        {
            var result = UseCase.AddLocation.Execute(new AddLocation.Request(CurrentUserName, bunchId, post.Name));
            return new LocationModel(result);
        }
    }
}