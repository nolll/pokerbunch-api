using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers.AppControllers
{
    public class AppGetController : BaseController
    {
        [Route(ApiRoutes.AppGet)]
        [HttpGet]
        [ApiAuthorize]
        public AppModel Get(int id)
        {
            var result = UseCase.GetApp.Execute(new GetApp.Request(CurrentUserName, id));
            return new AppModel(result);
        }
    }
}