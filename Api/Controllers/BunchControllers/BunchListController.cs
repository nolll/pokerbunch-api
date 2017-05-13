using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers.BunchControllers
{
    public class BunchListController : BaseController
    {
        [Route(ApiRoutes.BunchList)]
        [HttpGet]
        [ApiAuthorize]
        public BunchListModel List()
        {
            var request = new GetBunchList.AllBunchesRequest(CurrentUserName);
            var bunchListResult = UseCase.GetBunchList.Execute(request);
            return new BunchListModel(bunchListResult);
        }
    }
}
