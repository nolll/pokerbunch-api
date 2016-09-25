using System.Web.Http;
using Api.Auth;
using Api.Models;
using Core.UseCases;
using JetBrains.Annotations;
using Web.Common.Routes;

namespace Api.Controllers
{
    public class LocationController : BaseApiController
    {
        [Route(ApiRoutes.LocationList)]
        [AcceptVerbs(HttpVerb.Get)]
        [ApiAuthorize]
        public LocationListModel GetList(string slug)
        {
            var locationListResult = UseCase.GetLocationList.Execute(new GetLocationList.Request(CurrentUserName, slug));
            return new LocationListModel(locationListResult);
        }

        [Route(ApiRoutes.LocationGet)]
        [AcceptVerbs(HttpVerb.Get)]
        [ApiAuthorize]
        public IHttpActionResult Get(int id)
        {
            var result = UseCase.GetLocation.Execute(new GetLocation.Request(CurrentUserName, id));
            var locationModel = new LocationModel(result);
            return Ok(locationModel);
        }

        [Route(ApiRoutes.LocationAdd)]
        [AcceptVerbs(HttpVerb.Post)]
        [ApiAuthorize]
        public IHttpActionResult Add([FromBody] LocationModel location)
        {
            var result = UseCase.AddLocation.Execute(new AddLocation.Request(CurrentUserName, location.Bunch, location.Name));
            var locationModel = new LocationModel(result);
            return Ok(locationModel);
        }
    }
}
