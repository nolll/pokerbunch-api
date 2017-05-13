using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers.LocationControllers
{
    public class LocationAddController : BaseController
    {
        [Route(ApiRoutes.LocationAdd)]
        [HttpPost]
        [ApiAuthorize]
        public LocationModel Add([FromBody] LocationModel location)
        {
            var result = UseCase.AddLocation.Execute(new AddLocation.Request(CurrentUserName, location.Bunch, location.Name));
            return new LocationModel(result);
        }
    }
}
