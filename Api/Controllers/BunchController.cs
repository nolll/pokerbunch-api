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
        public BunchListModel List()
        {
            var bunchListResult = UseCase.GetBunchList.Execute(new GetBunchList.AllBunchesRequest(CurrentUserName));
            return new BunchListModel(bunchListResult);
        }

        [Route(ApiRoutes.BunchGet)]
        [AcceptVerbs("GET")]
        [ApiAuthorize]
        public BunchModel Get(string slug)
        {
            var getBunchResult = UseCase.GetBunch.Execute(new GetBunch.Request(CurrentUserName, slug));
            return new BunchModel(getBunchResult);
        }
    }
}
