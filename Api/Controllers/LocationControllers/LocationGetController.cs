using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers.LocationControllers
{
    public class LocationGetController : BaseController
    {
        [Route(ApiRoutes.LocationGet)]
        [HttpGet]
        [ApiAuthorize]
        public LocationModel Get(int id)
        {
            var result = UseCase.GetLocation.Execute(new GetLocation.Request(CurrentUserName, id));
            return new LocationModel(result);
        }
    }
}