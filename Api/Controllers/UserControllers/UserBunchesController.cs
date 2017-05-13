using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers.UserControllers
{
    public class UserBunchesController : BaseController
    {
        [Route(ApiRoutes.UserBunchList)]
        [HttpGet]
        [ApiAuthorize]
        public BunchListModel Bunches()
        {
            var bunchListResult = UseCase.GetBunchList.Execute(new GetBunchList.UserBunchesRequest(CurrentUserName));
            return new BunchListModel(bunchListResult);
        }
    }
}