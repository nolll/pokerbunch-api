using System.Web.Http;
using Api.Auth;
using Api.Models;
using Core.UseCases;
using Web.Common.Routes;

namespace Api.Controllers
{
    public class LocationController : BaseApiController
    {
        [Route(ApiRoutes.LocationList)]
        [AcceptVerbs("GET")]
        [ApiAuthorize]
        public LocationListModel GetList(string slug)
        {
            var locationListResult = UseCase.GetLocationList.Execute(new GetLocationList.Request(CurrentUserName, slug));
            return new LocationListModel(locationListResult);
        }

        [Route(ApiRoutes.LocationGet)]
        [AcceptVerbs("GET")]
        [ApiAuthorize]
        public IHttpActionResult Get(int id)
        {
            var result = UseCase.GetLocation.Execute(new GetLocation.Request(CurrentUserName, id));
            var locationModel = new LocationModel(result);
            return Ok(locationModel);
        }
    }
}
