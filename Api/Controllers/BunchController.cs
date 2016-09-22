using System.Web.Http;
using Api.Auth;
using Api.Models;
using Core.UseCases;
using Web.Common.Routes;

namespace Api.Controllers
{
    public class BunchController : BaseApiController
    {
        [Route(ApiRoutes.BunchList)]
        [AcceptVerbs("GET")]
        [ApiAuthorize]
        public ApiBunchList List()
        {
            var bunchListResult = UseCase.BunchList.Execute(new BunchList.UserBunchesRequest(CurrentUserName));
            return new ApiBunchList(bunchListResult);
        }

        [Route(ApiRoutes.BunchDetails)]
        [AcceptVerbs("GET")]
        [ApiAuthorize]
        public IHttpActionResult Details(string slug)
        {
            var bunchDetails = UseCase.BunchDetails.Execute(new BunchDetails.Request(CurrentUserName, slug));
            var bunchModel = new ApiBunch(bunchDetails.Id, bunchDetails.Slug, bunchDetails.BunchName);
            return Ok(bunchModel);
        }
    }
}
