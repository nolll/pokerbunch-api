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
        public BunchListModel GetList()
        {
            var bunchListResult = UseCase.GetBunchList.Execute(new GetBunchList.UserBunchesRequest(CurrentUserName));
            return new BunchListModel(bunchListResult);
        }

        [Route(ApiRoutes.BunchGet)]
        [AcceptVerbs("GET")]
        [ApiAuthorize]
        public IHttpActionResult Get(string slug)
        {
            var bunchDetails = UseCase.GetBunch.Execute(new GetBunch.Request(CurrentUserName, slug));
            var bunchModel = new BunchModel(bunchDetails.Id, bunchDetails.Slug, bunchDetails.BunchName);
            return Ok(bunchModel);
        }
    }
}
